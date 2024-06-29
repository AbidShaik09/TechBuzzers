using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Models;
using TeechBuzzersBank.Repository;

namespace TeechBuzzersBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;
        private LoanRepository _loan;
        private LoanPayablesRepository _payables;
        private TransactionRepository _transaction;
        private InsuranceRepository _insurance;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository (db);
            _account = new AccountRepository (db);
            _loan = new LoanRepository (db);
            _payables = new LoanPayablesRepository (db);
            _transaction = new TransactionRepository (db);
            _insurance = new InsuranceRepository (db);
            
        }

        [HttpGet("[Action]")]
        public IActionResult getRequest()
        {
            Request request = new Request();
            request.loanRequest= _db.loanRequests.Where(e => e.status.Equals("Pending")).ToList();
            List<LoanRequest> lr = new List<LoanRequest>( request.loanRequest);
            foreach(LoanRequest r in lr)
            {
                if (!_loan.checkLoan(r.loanId))
                {
                    r.status = "Deleted";
                    request.loanRequest.Remove(r);
                }
            }
            request.insuranceRequest = _db.insuranceRequests.Where(e => e.status.Equals("Pending")).ToList();
            List<InsuranceRequest> ir = new List<InsuranceRequest>( request.insuranceRequest);
            foreach (InsuranceRequest r in ir)
            {
                if (!_insurance.checkInsurance(r.insuranceId))
                {
                    r.status = "Deleted";
                    request.insuranceRequest.Remove(r);
                }
            }


            _db.SaveChanges();
            return Ok(request);

        }
        [HttpPost("[Action]")]
        public IActionResult approveInsurance([FromBody] ApproveInsurance approve)
        {
            InsuranceRequest ir = _db.insuranceRequests.FirstOrDefault(e => e.insuranceId.Equals(approve.InsuranceId));
            if (ir == null)
            {
                return BadRequest("Insurance Request Not Found!");
            }
            Insurance i = _insurance.GetInsurance(approve.InsuranceId);
            if (i.payables!=null && i.payables.Count > 0)
            {
                ir.status = "Approved";
                _db.SaveChanges();
                return Ok("Insurance was already approved ! Error has been corrected in records");

            }
            i.status = "Active";
            Account account = _account.GetAccount(ir.accountId);
            i=_insurance.ApplyInsurance(i, account);
            _db.SaveChanges();
            return Ok("Approved");

        }

        [HttpPost("[Action]")]
        public IActionResult approveLoan([FromBody] ApproveLoan loan )
        {
            LoanRequest lr = _db.loanRequests.FirstOrDefault(e=>e.loanId.Equals( loan.LoanId));
            if(lr == null)
            {
                return BadRequest("Loan Request Not Found!");
            }
            Loans l = _loan.GetLoan(loan.LoanId);
            if(l.Payables!=null && l.Payables.Count > 0)
            {
                lr.status = lr.status = "Approved";
                _db.SaveChanges();

                return Ok("Loan was already approved ! Error has been corrected in records");
            }
            l.Status = "Active";
            Account adminAcc = _account.GetAccount("ACN42833749");
            DateTime dateTime = DateTime.Now.AddMonths(1);
            Account a = _account.GetAccount(l.AccountId);
            for (int i = 0; i < l.Tenure; i++)
            {
                l = _payables.generateLoanPayables(l, dateTime, i);
                dateTime = dateTime.AddMonths(1);
            }

            if (a.Transactions == null)
            {
                a.Transactions = new List<string>();
            }
            a.Transactions.Add(_transaction.transfer(adminAcc, a, l.LoanAmount, "BankToUser Transfer").Id);
            adminAcc.Balance += l.LoanAmount;
            lr.status = "Approved";


            _db.SaveChanges();

            return Ok(lr.status);

        }


        [HttpPost("[Action]")]
        public IActionResult rejectInsurance([FromBody] ApproveInsurance reject)
        {

            InsuranceRequest ir = _db.insuranceRequests.FirstOrDefault(e => e.insuranceId.Equals(reject.InsuranceId));
            if (ir == null)
            {
                return BadRequest("Insurance Request Not Found!");
            }
            Insurance i = _insurance.GetInsurance(reject.InsuranceId);
            i.status = "Rejected";
            ir.status = "Rejected";
            _db.SaveChanges();
            return Ok(ir.status);

        }


            [HttpPost("[Action]")]
        public IActionResult rejectLoan([FromBody] ApproveLoan loan)
        {
            LoanRequest lr = _db.loanRequests.FirstOrDefault(e => e.loanId.Equals(loan.LoanId));
            if (lr == null)
            {
                return BadRequest("Loan Request Not Found!");
            }
            Loans l = _loan.GetLoan(loan.LoanId);
            l.Status = "Rejected";
            lr.status = "Rejected";
            lr.loanId = null;
           
            _db.SaveChanges();

            return Ok(lr.status);
        }


        public class ApproveLoan
        {
            public string LoanId { get; set; }
        }
        public class ApproveInsurance
        {
            public string InsuranceId { get; set; }
        }
        public class Request
        {
            public List<LoanRequest> loanRequest { get; set; }
            public List<InsuranceRequest> insuranceRequest { get; set; }


        }
    }
}
