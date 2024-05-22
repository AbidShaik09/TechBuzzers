using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class AllUserDetails
    {
        public string userId { get; set; }
        public string primaryAccountId { get; set; }
        public UserDetails userDetails { get; set; }
        public List<Account> accounts { get; set; }
        public List<Loans> loans { get; set; }

        public List<Bill> bills { get; set; }
        public List<LoanPayables> loanPayables { get; set; }
        public AllUserDetails(UserDetails u)
        {
            userDetails = u;
            userId = u.Id;
            bills = u.bills;

        }
    }

}
