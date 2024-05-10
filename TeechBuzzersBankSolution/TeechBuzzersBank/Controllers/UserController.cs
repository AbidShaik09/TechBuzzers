using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Techbuzzers_bank.Controllers
{
    [Route("/[controller]/")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;
        public UserController( ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);

        }

        [HttpGet("/[Action]")]
        public IActionResult GetUserDetails()
        {
            try
            {
                // Retrieve the JWT token from the HTTP request headers
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                // Parse the JWT token to extract the claims
              
                var userId = getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }
                AllUserDetails allUserDetails = new AllUserDetails(user);
                allUserDetails.accounts = _account.GetAllAccounts(userId);
                return Ok(allUserDetails);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }


        


        // GET: api/<UserController>
        [HttpGet("/[Action]")]
        public IActionResult GetBankAccounts()
        {
            try
            {

                // Retrieve the JWT token from the HTTP request headers
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId=getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user != null)
                {
                    try
                    {
                        return Ok(_account.GetAllAccounts(user.Id));

                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }


                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(405,ex.Message);
            }

           
        }

        [HttpPost("/[Action]")]
        public IActionResult CreateBankAccount( string accountName)
        {
            try
            {
                // Retrieve the JWT token from the HTTP request headers
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                // Parse the JWT token to extract the claims

                var userId = getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user != null)
                {
                    try
                    {

                        _account.CreateNewAccount(user.Id, 0, accountName);
                        return Ok(_account.GetAllAccounts(user.Id));
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }

                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(405, ex.Message);
            }

        }


        private class AllUserDetails
        {
            public string userId {  get; set; }
            public UserDetails userDetails { get; set;}
            public List<Account> accounts { get; set;}
            public AllUserDetails(UserDetails u) { 
                userDetails = u;
                userId=u.Id;
            }
        }


       
        public static string getIdFromToken(string Token)
        {
            // Retrieve the JWT token from the HTTP request headers
            var token = Token;

            // Parse the JWT token to extract the claims
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Find the claim with the name "UserId" and extract its value
            var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");

            if (userIdClaim == null)
            {
                throw new Exception("UserId claim not found in the JWT token.");
            }

            var userId = userIdClaim.Value;
            return userId;

        }

    }
}
