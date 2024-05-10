using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Models;

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
              
                var userId = _user.getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }
                AllUserDetails allUserDetails = new AllUserDetails(user);
                allUserDetails.accounts = _account.GetAllAccounts(userId);
                allUserDetails.primaryAccountId = user.PrimaryAccountId;
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
                var userId= _user.getIdFromToken(token);
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
        public IActionResult CreateBankAccount([FromBody] AccountDetails accountName)
        {
            try
            {
                // Retrieve the JWT token from the HTTP request headers
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                // Parse the JWT token to extract the claims

                var userId = _user.getIdFromToken(token);
                UserDetails user = _user.GetUserDetails(userId);
                if (user != null)
                {
                    try
                    {

                        _account.CreateNewAccount(user.Id, 0, accountName.accountName,false);
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




        [HttpPost("/[Action]")]
        public IActionResult SetPrimaryAccount([FromBody] SetPrimary accountDetails)
        {
            string accId =accountDetails.accountId;
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            string userId = _user.getIdFromToken(token);
            UserDetails user = _user.GetUserDetails(userId);
            Account account = _account.GetAccount(accId);
            _account.setPrimaryAccount(user, account);
            return Ok("Done");
        }
        
        public class AccountDetails
        {
            public string accountName { get; set; }
        }

        public class SetPrimary
        {
            public string accountId { get; set; }
        }

    }
}
