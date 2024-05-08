using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Techbuzzers_bank.Controllers
{
    [Route("/[controller]")]
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
        public IActionResult GetUserDetails(string userId)
        {
            try
            {
                UserDetails userDetails = _user.GetUserDetails(userId);
                if (userDetails == null)
                {
                    return NotFound();
                }
                userDetails.PhoneNumber = 0;
                userDetails.PANNumber = "hidden";
                userDetails.AdhaarNumber = 0;
                userDetails.accounts = new List<string>();
                userDetails.Address = "hidden";
                userDetails.Pin = 0;
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }


        [HttpPost("/[Action]")]
        public IActionResult GetAllUserDetails([FromBody]UserCred userCred )
        {
            try
            {
                UserDetails user = _user.GetUser(userCred.PhoneNumber, userCred.Pin);
                if (user != null)
                {
                    try
                    {
                        user.Pin = 0;
                        AllUserDetails allUserDetails = new AllUserDetails(user);
                        allUserDetails.accounts = _account.GetAllAccounts(user.Id);
                        return Ok(allUserDetails);

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
                return StatusCode(404,ex.Message);
            }
            
        }


        // GET: api/<UserController>
        [HttpPost("/[Action]")]
        public IActionResult GetUserAccounts([FromBody]UserCred userCred)
        {
            try
            {
                UserDetails user = _user.GetUser(userCred.PhoneNumber, userCred.Pin);
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
        public IActionResult CreateNewUserAccount([FromBody]UserCred userCred, string accountName)
        {
            try
            {

                UserDetails user = _user.GetUser(userCred.PhoneNumber, userCred.Pin);
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


       
        
    }
}
