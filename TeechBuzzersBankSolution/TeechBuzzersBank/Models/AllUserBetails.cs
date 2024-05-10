using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class AllUserDetails
    {
        public string userId { get; set; }
        public string primaryAccountId { get; set; }
        public UserDetails userDetails { get; set; }
        public List<Account> accounts { get; set; }
        public AllUserDetails(UserDetails u)
        {
            userDetails = u;
            userId = u.Id;
        }
    }

}
