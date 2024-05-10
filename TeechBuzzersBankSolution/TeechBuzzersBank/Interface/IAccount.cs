using Techbuzzers_bank.Models;

namespace Techbuzzers_bank.Interface
{
    public interface IAccount
    {
        public List<Account> GetAllAccounts(string userId);
        public Account GetAccount(string accountId);
        public Account AddAccount(Account accountDetails);
        public Account CreateNewAccount(string userId, float balance,string accountName, bool isPrimary=false);
        public void setPrimaryAccount(UserDetails user, Account account);
        public void UpdateAccount(Account accountDetails);
        public Account DeleteAccount(string accountId);
        public bool CheckAccount(string accountId);
    }
}
