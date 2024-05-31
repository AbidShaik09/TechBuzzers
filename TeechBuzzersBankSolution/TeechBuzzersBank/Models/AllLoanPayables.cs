using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class AllLoanPayables
    {
        public List< LoanPayables> pending { get; set; } = new List< LoanPayables>();
        public List< LoanPayables> due { get; set; } = new List<LoanPayables>();
        public List<LoanPayables> paid { get; set; } = new List<LoanPayables>();

    }
}
