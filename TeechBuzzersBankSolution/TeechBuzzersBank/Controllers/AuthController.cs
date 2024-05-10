using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;

namespace Techbuzzers_bank.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration _config;
        private readonly IUsers _user;
        private readonly IAccount _account;
        private readonly ApplicationDbContext _db;
        public AuthController( IConfiguration config, ApplicationDbContext db) { 
            _db= db;
            _config = config;
            _user= new UserRepository(db);
            _account= new AccountRepository(db);
        }


        [HttpPost("/signup")]
        public async Task<IActionResult> signUp([FromBody]UserDetails userDetails)
        {

            if (userDetails == null)
            {
                return BadRequest("Userdetails not recieved");
            }
            var dbuser = _db.userDetails.FirstOrDefault(e => e.PhoneNumber == userDetails.PhoneNumber || e.Email == userDetails.Email || e.AdhaarNumber == userDetails.AdhaarNumber || e.PANNumber.Equals(userDetails.PANNumber));
            if (dbuser != null)
            {
                if(dbuser.Email == userDetails.Email)
                {
                    return BadRequest("Email is already registered");
                }
                else if (dbuser.PhoneNumber == userDetails.PhoneNumber)
                {
                    return BadRequest("Phone number is already registered");
                }
                else if (dbuser.AdhaarNumber == userDetails.AdhaarNumber)
                {
                    return BadRequest("Adhaar number is already registered");
                }
                else {
                    return BadRequest("PAN number is already registered");
                }

            }
            else
            {
                userDetails.accounts = new List<Account>();
               
                _user.AddUser(userDetails);
                Account acc = _account.CreateNewAccount(userDetails.Id, 5000,"DefaultAccount");
                userDetails.PrimaryAccountId = acc.Id;
                _user.UpdateUser(userDetails);
                return Ok();
            }
        }

        [HttpPost("/signin")]
        public async Task<IActionResult> signIn(UserCred userDetails)
        {
            if (userDetails != null && userDetails.PhoneNumber != null && userDetails.Pin!= null)
            {
                try
                {

                    UserDetails user = _user.GetUser(userDetails.PhoneNumber, userDetails.Pin);

                    if (user != null)
                    {

                        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("DisplayName", user.FirstName),
                        new Claim("UserName", user.LastName),
                        new Claim("Email", user.Email)
                    };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                            _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(10),
                            signingCredentials: signIn);
                        SigninResponse response = new SigninResponse();
                        response.message = "sign-in success";
                        response.token = new JwtSecurityTokenHandler().WriteToken(token);
                        return Ok(response);

                    }
                    else
                    {
                        return BadRequest("Invalid credentials");
                    }

                }
                catch(Exception ex)
                {
                    return StatusCode(405, ex.Message);
                }
                
            }
            else
            {
                return BadRequest("Some essential value is  null ");
            }

        }
    }
    public class UserCred
    {
        public long PhoneNumber { get; set; }
        public int Pin { get; set; }
    }
    public class SigninResponse
    {
        public string message { get; set; }
        public string token { get; set; }
    }
}
