using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface ILoans
    {
        public List<LoanDetails> getLoanDetails();

        public Loans applyLoan(Loans loanData,string userId);

        public Loans GetLoan(string loanId);

        public bool checkLoan(string loanId);

        public void deleteLoan(string loanId);

        public List<Loans> getLoansOfPhoneNumber(long userPhoneNumber);

        public List<Loans> getActiveLoansOfUser(string userId);
        public bool checkLoanData(string loanDetailsId);

        public List<LoanDetails> getLoanDetails(string loanType="Personal Loan");
        public LoanDetails getLoanDetailsFromId(string loanDetailsId);
        public List<Loans> GetLoansOfAccount(string accountId);


    }
}
