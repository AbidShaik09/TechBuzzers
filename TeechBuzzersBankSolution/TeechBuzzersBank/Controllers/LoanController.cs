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

                List<LoanDetails> ld = _loan.getLoanDetails(loanType);
                List<LoanDetailsWithTenure> ldt = new List<LoanDetailsWithTenure>();
                foreach (LoanDetails l in ld)
                {
                    LoanDetailsWithTenure t = new LoanDetailsWithTenure(l);
                    ldt.Add(t);
                }
                return Ok(ldt);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        public class LoanDetailsWithTenure
        {
            public LoanDetails loanDetails { get; set; }
            public int[] tenure { get; set; }
            public LoanDetailsWithTenure(LoanDetails l)
            {
                int count = 0;
                loanDetails = l;
                if(l.MaxLoanTenure%3!=0)
                    count++;
                for(int i = 3; i <= l.MaxLoanTenure; i += 3)
                {
                    count += 1;
                }
                tenure = new int[count];
                for(int i = 0; i < count; i+=1)
                {
                    tenure[i] = (1 + i) * 3;
                }
                tenure[ count-1] =l.MaxLoanTenure;


            }
        }
        [HttpGet("/[Action]")]
        public IActionResult GetAllLoanDetails()
        {

            try
            {

                List<LoanDetails> ld = _loan.getLoanDetails();
                List<LoanDetailsWithTenure> ldt = new List<LoanDetailsWithTenure>();
                foreach (LoanDetails l in ld)
                {
                    LoanDetailsWithTenure t = new LoanDetailsWithTenure(l);
                    ldt.Add(t);
                }
                return Ok(ldt);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


        [HttpGet("/[Action]")]
        public IActionResult getupcomingInstallments()
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

                List<LoanPayables> loanPayables = _payables.getUpcomingPayables(userId);
                return Ok(loanPayables);

            }
            catch(Exception ex)
            {

                return BadRequest(ex.Message);
            }

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
            try
            {

                if (!_loan.checkLoan(loanID))
                {
                    return BadRequest("Invalid LoanId");
                }
                Loans l = _loan.GetLoan(loanID);
                LoanStatus ls = new LoanStatus(l);
                return Ok(

                    ls
                    );

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        public class ApplyLoanFormat
        {
            public Loans loan { get; set; }
            public int pin { get; set; }
        }


        [HttpDelete("/[Action]")]
        public IActionResult DeleteLoan(string LoanId)
        {
            try
            {

                _loan.deleteLoan(LoanId);
                return Ok("Loan Has been deleted");

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       [HttpPost("/[Action]")]
        public IActionResult ApplyLoan([FromBody] ApplyLoanFormat lan)
        {

            try
            {


                Loans loanData = lan.loan;
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                var userId = _user.getIdFromToken(token);

                UserDetails user = _user.GetUserDetails(userId);
                if (user == null)
                {
                    return NotFound();
                }
                if (user.Pin != lan.pin)
                {
                    return Unauthorized("Invalid Pin");
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
                return Ok(_loan.applyLoan(loanData, userId));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

          
        }

    }

    public class LoanInstallmentDetails{

        public string payablesId { get;set; }
        public string accountId { get;set; }

    
    }

   
}
