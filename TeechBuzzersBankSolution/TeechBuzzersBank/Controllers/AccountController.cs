using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using static Techbuzzers_bank.Controllers.UserController;

namespace TeechBuzzersBank.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;

        public AccountController( ApplicationDbContext db)
        {
            _db = db;
            _account=new AccountRepository(db);
            _user=new UserRepository(db);

            
        }

        [HttpGet("/[Action]")]
        public IActionResult GetBankBalance(string accountId)
        {
            Account account= _account.GetAccount(accountId);
            
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            var userId = _user.getIdFromToken(token);
            UserDetails user = _user.GetUserDetails(userId);
            if(user == null)
            {
                return BadRequest("Invalid User, Login Again");
            }
            if(account.UserId != userId) {
                return  Unauthorized("Can't see balance of other user accounts!");
            }
            return Ok(account.Balance);


        }

        // GET: api/<UserController>
        [HttpGet("/[Action]")]
        public IActionResult GetBankAccounts()
        {
            try
            {

                // Retrieve the JWT token from the HTTP request headers
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var userId = _user.getIdFromToken(token);
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
                return StatusCode(405, ex.Message);
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

                        _account.CreateNewAccount(user.Id, 0, accountName.accountName, false);
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
                return StatusCode(405, ex.Message);
            }

        }




        [HttpPost("/[Action]")]
        public IActionResult SetPrimaryAccount([FromBody] SetPrimary accountDetails)
        {
            string accId = accountDetails.accountId;
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
