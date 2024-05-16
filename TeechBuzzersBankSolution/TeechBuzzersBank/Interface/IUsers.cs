using System.Numerics;
using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace Techbuzzers_bank.Interface
{
    public interface IUsers
    {
        public List<UserDetails> GetAllUserDetails();
        public UserDetails GetUserDetails(string id);
        public void AddUser(UserDetails userDetails);
        public bool CheckUser(long phone);
        public UserDetails GetUser(long PhoneNumber, int Pin);
        public void UpdateUser(UserDetails userDetails);
        public UserDetails DeleteUser(string id);
        public List<Account> GetAllUserAccounts(string userId);
        public string getIdFromToken(string Token);
        public bool CheckUser(string id);
        public void AddLoanToUser(Loans loanData,string userId);
        public PublicUserDetails getPublicDetails(UserDetails user, long phone);
    }
}
