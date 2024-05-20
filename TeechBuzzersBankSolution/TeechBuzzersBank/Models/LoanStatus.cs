using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class LoanStatus
    {
        public string loanId {  get; set; } 
        public Loans loan { get; set; }
        public float dueAmount { get; set; }
        public int allTenures { get; set; }
        public int paidTenures { get; set; }
        public List<Transactions> paidTenureRecipts { get; set; }
        public LoanStatus(Loans l) {
            loan = l;
            loanId = l.Id;
            dueAmount = loan.Due;
            allTenures = loan.Tenure;
            paidTenures = loan.paidTenures;

            paidTenureRecipts = loan.paidTenuresList;
        }
    }
}
