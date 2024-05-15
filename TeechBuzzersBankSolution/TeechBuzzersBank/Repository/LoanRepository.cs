using Techbuzzers_bank.Data;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Repository
{
    public class LoanRepository:ILoans
    {

        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;

        public LoanRepository(ApplicationDbContext db)
        {
            _db = db;
            _account= new AccountRepository(db);
            _user= new UserRepository(db);
        }
        public List<LoanDetails> getLoanDetails()
        {
            return _db.loanDetails.Where(e=>e.Id==e.Id).ToList();
        }

        public void applyLoan(Loans loanData)
        {
            loanData.Id ="LNO"+ GenerateUniqueLoanId();
            if (!checkLoanData(loanData.loanDetailsId))
            {
                throw new Exception("Select a valid LoanId!");
            }

            LoanDetails loanDetails = getLoanDetails(loanData.loanDetailsId);
            loanData.Tenure = loanDetails.LoanTenure;

        }

        public bool checkLoanData(string loanDetailsId)
        {

            return _db.loanDetails.Find(loanDetailsId) != null;
        }

        public LoanDetails getLoanDetails(string loanDetailsId)
        {
            return _db.loanDetails.Find(loanDetailsId);
        }

        public bool checkLoan(string loanId)
        {
            return _db.loans.Find(loanId)!=null;
        }
        public List<Loans> getLoansOfPhoneNumber(long userPhoneNumber)
        {
            List<Loans> loans = new List<Loans>();
            UserDetails user = _db.userDetails.Where(e=>e.PhoneNumber==userPhoneNumber).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("User not found!");
            }

            List<Account> accounts = _db.account.Where(e=>e.UserId==user.Id).ToList();
            foreach(Account a in accounts)
            {
                List<Loans> l= GetLoansOfAccount(a.Id);
                foreach (Loans l1 in l)
                {
                    loans.Add(l1);
                }
            }
            return loans;

        }

        public List<Loans> getActiveLoansOfUser(string userId)
        {
            if (!_user.CheckUser(userId))
            {
                throw new Exception("User not found!");
            }
            List<Account> accounts= _db.account.Where(e=> e.UserId==userId).ToList();
            


            List<Loans> loans = new List<Loans>();
            foreach (Account a in accounts)
            {
                List<Loans> l = _db.loans.Where(e=>e.Status.Equals("Active")).ToList();
                foreach (Loans l1 in l)
                {
                    loans.Add(l1);
                }
            }
            return loans;
        }

        public List<Loans> GetLoansOfAccount(string accountId)
        {
            List<Loans> loans=_db.loans.Where(e=>e.AccountId==accountId).ToList();
            return loans;

        }

        private long GenerateUniqueLoanId()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (checkLoan("LNO" + id));
            return id;

        }

    }
}
