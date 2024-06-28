using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;
using TeechBuzzersBank.Models;
using TeechBuzzersBank.Repository;
using static Techbuzzers_bank.Controllers.UserController;


namespace Techbuzzers_bank.Controllers
{
    [Route("/[controller]/")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private IUsers _user;
        private IAccount _account;
        private ILoans _loan;
        private ILoanPayables _payables;
        public UserController( ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);
            _loan = new LoanRepository(db);
            _payables = new LoanPayablesRepository(db);
        }

        [HttpGet("/[Action]")]
        public IActionResult GetUserDetails()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
     
                var userId = _user.getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }

                AllUserDetails allUserDetails = new AllUserDetails(user);
                allUserDetails.accounts = _account.GetAllAccounts(userId);
                allUserDetails.primaryAccountId = user.PrimaryAccountId;

                List<Loans> loans = _loan.getActiveLoansOfUser(userId);

                List<LoansResultFormat> loansResultFormats = new List<LoansResultFormat>();

                foreach (Loans l in loans)
                {
                    LoansResultFormat ls = new LoansResultFormat(l);
                    ls.loanType = _loan.getLoanDetailsFromId(ls.loanDetailsId).LoanType;
                    ls.ROI = _loan.getLoanDetailsFromId(ls.loanDetailsId).ROI;

                    loansResultFormats.Add(ls);
                }
                allUserDetails.loans = loansResultFormats;
                allUserDetails.loanPayables = _payables.getUpcomingPayables(userId);
                
                return Ok(allUserDetails);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }


        private class SortLoanPayables : IComparer<LoanPayables>
        {
            int IComparer<LoanPayables>.Compare(LoanPayables a, LoanPayables b)
            {
                if (a.dueDate < b.dueDate) return -1;
                if (a.dueDate > b.dueDate) return 1;
                return 0;
            }
        }

        [HttpPut("/[Action]")]
        public async Task<IActionResult> UpdateUser ([FromBody] UserDetails newUserDetails)
        {
            
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            string userId = _user.getIdFromToken(token);
            UserDetails oldDetails = _user.GetUserDetails(userId);
            
            if (newUserDetails == null)
            {
                return BadRequest("Essential User Details are missing");

            }

            

            if(!_user.CheckUser(newUserDetails.PhoneNumber))
                oldDetails.PhoneNumber = newUserDetails.PhoneNumber;
            oldDetails.Address = newUserDetails.Address;
            oldDetails.Gender= newUserDetails.Gender;
            oldDetails.FirstName= newUserDetails.FirstName;
            oldDetails.LastName = newUserDetails.LastName;
            _db.SaveChanges();
            return Ok(oldDetails);
        }

        public class VerifyUser
        {
            public int pin { get; set; }

        }
        [HttpPost("/[Action]")]
        public IActionResult verifyUserPin([FromBody] VerifyUser userPin)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            string userId = _user.getIdFromToken(token);
            UserDetails user = _user.GetUserDetails(userId);
            if (user == null)
            {
                return BadRequest("Session Expired, Login again!");
            }
            if (user.Pin != userPin.pin)
            {
                return Unauthorized(false);
            }
            else
            {
                return Ok(true);
            }
        }
    }
}
