using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Models;
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
        private LoanPayablesRepository _payables;
        public LoanController(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);
            _loan = new LoanRepository(db);
            _payables = new LoanPayablesRepository(db);
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


        [HttpGet("/[Action]")]
        public IActionResult getupcomingInstallments()
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

            List<LoanPayables> loanPayables = _payables.getUpcomingPayables(userId);
            return Ok(loanPayables);
        }

        [HttpPost("/[Action]")]
        public IActionResult PayLoanInstallment([FromBody]LoanInstallmentDetails loanInstallment )
        {
            try
            {

                if (!_payables.checkPayables(loanInstallment.payablesId) || !_account.CheckAccount(loanInstallment.accountId))
                {
                    return BadRequest("Invalid accountId / loanPayableId");
                }



                LoanPayables loanPayable = _payables.getLoanPayable(loanInstallment.payablesId);
                if (loanPayable == null)
                {
                    return BadRequest("LoanPayable is NULL / Contact Admin");
                }


                Loans loan = _loan.GetLoan(loanPayable.LoanId);
                if (loan == null)
                {

                    return BadRequest("Loan is NULL / Contact Admin");
                }
                if (loan.Status == "Clear")
                {
                    return BadRequest("Loan has already been paid!");
                }
                Account account = _account.GetAccount(loanInstallment.accountId);
                Account loanAccount = _account.GetAccount(loan.AccountId);
                if (account.UserId != loanAccount.UserId)
                {
                    return BadRequest("Account is not owned by the loan holder!");
                }

                Transactions t = _payables.payInstallment(loanPayable, account);
                loan.paidTenures += 1;
                loan.Due =loan.Due- loanPayable.Amount;
                if (loan.paidTenuresList == null)
                {
                    loan.paidTenuresList = new List<Transactions>();
                }
                loan.paidTenuresList.Add(t);    
                if (loan.paidTenures >= loan.Tenure)
                {

                    loan.Status = "Clear";
                }
                _db.SaveChanges();
                return Ok(t);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }


        [HttpGet("/[Action]")]
        public IActionResult getLoanStatus(string loanID)
        {

            if(!_loan.checkLoan(loanID))
            {
                return BadRequest("Invalid LoanId");
            }
            Loans l = _loan.GetLoan(loanID);
            LoanStatus ls = new LoanStatus(l);
            return Ok(
                 
                ls
                );
        }
    
        [HttpGet("/[Action]")]
        public IActionResult getAllLoans()
        {

            try {

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                var userId = _user.getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }

                List<LoanStatus> loanStatus = new List<LoanStatus>();

                List<Loans> loans= _loan.getActiveLoansOfUser(userId);

                foreach(Loans l in loans)
                {
                    LoanStatus ls = new LoanStatus(l);
                    

                   loanStatus.Add(ls);
                }

                return Ok(loanStatus);
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
            if (account == null)
            {
                return BadRequest("Account Not Found!");
            }
            if (user.Id != account.UserId)
            {
                return BadRequest("Account Does not belong to user!");
            }

            try
            {
                return Ok(_loan.applyLoan(loanData, userId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }

    public class LoanInstallmentDetails{

        public string payablesId { get;set; }
        public string accountId { get;set; }

    
    }
}
