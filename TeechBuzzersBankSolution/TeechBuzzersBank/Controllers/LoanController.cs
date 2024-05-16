using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Repository;

namespace TeechBuzzersBank.Controllers
{
    
    [Route("/")]
    [Authorize]
    [ApiController]
    public class LoanController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;
        private LoanRepository _loan;
        public LoanController(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);
            _loan = new LoanRepository(db); 
        }

        [HttpGet("/[Action]")]
        public IActionResult GetLoanDetails(string loanType)
        {

           
            try
            {
                return Ok(_loan.getLoanDetails(loanType));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("/[Action]")]
        public IActionResult GetAllLoanDetails()
        {

            try
            {
                return Ok(_loan.getLoanDetails());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("/[Action]")]
        public IActionResult ApplyLoan([FromBody] Loans loanData)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            var userId = _user.getIdFromToken(token);

            UserDetails user = _user.GetUserDetails(userId);
            if (user == null)
            {
                return NotFound();
            }
            Account account = _account.GetAccount(loanData.AccountId);
            if(account == null)
            {
                                        return BadRequest("Account Not Found!");
            }
            if (user.Id != account.UserId)
            {
                return BadRequest("Account Does not belong to user!");
            }

            try
            {
                return Ok(_loan.applyLoan(loanData,userId));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
