using Microsoft.EntityFrameworkCore;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;

namespace Techbuzzers_bank.Repository
{
    public class AccountRepository: Interface.IAccount
    {

        readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<Account> GetAllAccounts(string userId)
        {
            List<Account> accounts = _db.account.Where(e=>e.UserId.Equals(userId)).ToList();
            return accounts;
        }
        public Account GetAccount(string accountId)
        {

            Account account = _db.account.Find(accountId);
            
            return account;
        }
        public Account AddAccount(Account accountDetails)
        {
            List<Account> userAccounts = GetAllAccounts(accountDetails.UserId);

            if(userAccounts!=null && userAccounts.Count > 0)
            {
                foreach (Account account in  userAccounts)
                {
                    if (account.accountName.Equals(accountDetails.accountName))
                    {
                        throw new Exception("There's another account named " + accountDetails.accountName + ". Use another account Name");
                    }
                }
            }

            if (accountDetails != null)
            {
                accountDetails.Id = "ACN"+ GenerateUniqueAccountId();

                _db.account.Add(accountDetails);
                UserDetails u = _db.userDetails.Find(accountDetails.UserId);
                if (u!=null)
                {
                    u.accounts.Add(accountDetails.Id);
                }
                _db.SaveChanges();
                return accountDetails;
            }
            else
            {
                throw new Exception("accountDetails can't be null");
            }
        }
        public Account CreateNewAccount(string userId, float balance,string accountName)
        {
            Account account = new Account();
            account.UserId = userId;
            account.Balance = balance;
            account.accountName= accountName;
            try
            {
                account = AddAccount(account);

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return account;

        }
        public void UpdateAccount(Account accountDetails)
        {
            if (accountDetails != null && CheckAccount(accountDetails.Id) )
            {
                _db.account.Update(accountDetails);
                _db.SaveChanges();
            }
            else 
            {
                throw new Exception("account not found!");

            }
        }
        public Account DeleteAccount(string accountId)
        {
            if (accountId != null && CheckAccount(accountId))
            {
                Account account = _db.account.Find(accountId);
                _db.account.Remove(account);
                _db.SaveChanges();
                return account;
            }
            else
            {
                throw new Exception("account not found!");

            }
        }
        public bool CheckAccount(string accountId)
        {
            return _db.account.Find(accountId) != null;
        }


        private long GenerateUniqueAccountId()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (CheckAccount("ACN"+id));
            return id;

        }
    }
}
