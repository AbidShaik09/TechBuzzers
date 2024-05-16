using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface ILoanPayables
    {
        //public List<LoanPayables> getUpcomingPayables(string userId);

        public bool checkPayables(string payablesId);
    }
}
