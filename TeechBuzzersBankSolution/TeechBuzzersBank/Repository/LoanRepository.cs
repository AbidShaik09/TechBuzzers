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
        private LoanPayablesRepository _payables;
        public LoanRepository(ApplicationDbContext db)
        {
            _db = db;
            _account= new AccountRepository(db);
            _user= new UserRepository(db);
            _payables = new LoanPayablesRepository(db);
        }
        public List<LoanDetails> getLoanDetails()
        {
            return _db.loanDetails.ToList();
        }
        public List<LoanDetails> getLoanDetails(string loanType)
        {
            return _db.loanDetails.Where(e=>e.LoanType.Equals(loanType)).ToList();
        }

        public Loans applyLoan(Loans loanData, string userId)
        {

            Account a = _account.GetAccount(loanData.AccountId);
            loanData.Id ="LNO"+ GenerateUniqueLoanId();
            if (!checkLoanData(loanData.loanDetailsId))
            {
                throw new Exception("Select a valid LoanId!");
            }
            UserDetails user= _user.GetUserDetails(userId);
            LoanDetails loanDetails = getLoanDetailsFromId(loanData.loanDetailsId);
            if (loanData.LoanAmount > loanDetails.AmouuntGranted)
            {
                throw new Exception("can't grant more than the Maximum applicable amount of LoanType");
            }
            if (a == null)
            {
                throw new Exception("invalid Bank Account!");

            }
            loanData.Due = loanData.LoanAmount;
            loanData.TenureAmount = (loanData.LoanAmount * (1 + (loanDetails.ROI*loanData.Tenure)/(100*12)))/loanData.Tenure;
            
            DateTime dateTime = DateTime.Now.AddMonths(1);
            loanData.Payables = new List<LoanPayables>();
            for (int i = 0; i < loanData.Tenure;i++)
            {
                loanData=_payables.generateLoanPayables(loanData,dateTime,i);
                dateTime=dateTime.AddMonths(1);

            }
            loanData.LoanType=loanDetails.LoanType;
            loanData.Status = "Active";
            loanData.Timestamp = DateTime.Now;
            loanData.Due = (loanData.LoanAmount * (1 + (loanDetails.ROI * loanData.Tenure) / (100 * 12)));
            loanData.LoanAmount = loanData.LoanAmount;
            //add transaction from techBuzzers Bank account
            a.Balance += loanData.LoanAmount;
            user.loans.Add(loanData);

            _db.SaveChanges();
            return loanData;
        }

        public bool checkLoanData(string loanDetailsId)
        {

            return _db.loanDetails.Find(loanDetailsId) != null;
        }

        public LoanDetails getLoanDetailsFromId(string loanDetailsId)
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
                List<Loans> l = _db.loans.Where(e=> e.AccountId.Equals(a.Id)  &&e.Status.Equals("Active")).ToList();
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
