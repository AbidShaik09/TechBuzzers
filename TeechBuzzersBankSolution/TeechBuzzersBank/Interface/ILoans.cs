using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface ILoans
    {
        public List<LoanDetails> getLoanDetails();

        public void applyLoan(Loans loanData);

        public bool checkLoan(string loanId);
        public List<Loans> getLoansOfPhoneNumber(long userPhoneNumber);

        public List<Loans> getActiveLoansOfUser(string userId);
        public bool checkLoanData(string loanDetailsId);

        public LoanDetails getLoanDetails(string loanDetailsId);
        public List<Loans> GetLoansOfAccount(string accountId);
    }
}
