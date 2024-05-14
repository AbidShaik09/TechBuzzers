using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class PublicUserDetails
    {


        public string userId { get; set; }

        public string primaryAccountId { get; set; }
        public long phoneNumber { get; set; }
        public string name { get; set; }
        public List<Transactions> transactions { get; set; }


    }
}
