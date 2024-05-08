using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;

namespace Techbuzzers_bank.Repository
{
    public class UserRepository:IUsers
    {
        readonly ApplicationDbContext _db ;
        private AccountRepository _account;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
            _account= new AccountRepository(db);
        }   

        public List<UserDetails> GetAllUserDetails()
        {
            try
            {
                return _db.userDetails.ToList();
            }
            catch
            {
                throw;
            }
        }

        public UserDetails GetUserDetails(string id)
        {
            try
            {
                UserDetails? user = _db.userDetails.Find(id);
                if (user == null)
                {
                    throw new Exception("User Not Found!");
                }
                return user;
            }
            catch
            {
                throw;
            }
        }
        public void AddUser(UserDetails user)
        {
            try
            {
                user.Id ="USR"+ GenerateUniqueUserId();
                _db.userDetails.Add(user);
                _db.SaveChanges();

            }
            catch
            {
                throw;
            }
        }



        public void UpdateUser(UserDetails user)
        {
            try
            {
                _db.userDetails.Update(user);
                _db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public UserDetails DeleteUser(string id)
        {
            try{
                UserDetails? user = _db.userDetails.Find(id);
                if(user == null)
                {
                    throw new Exception("User Does  not exist!");
                }
                else
                {
                    foreach (var accountId in user.accounts)
                    {
                        _account.DeleteAccount(accountId);
                        user.accounts.Remove(accountId);
                    }
                    _db.userDetails.Remove(user);
                    _db.SaveChanges();
                    return user;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckUser(string id)
        {
            return _db.userDetails.Any(e=>e.Id == id);
        }
        public UserDetails GetUser(long PhoneNumber,int Pin)
        {
            UserDetails user= _db.userDetails.FirstOrDefault(e=>e.PhoneNumber== PhoneNumber && e.Pin==Pin);
            if(user == null)
            {
                throw new Exception("Invalid Credentials");
            }
            else
            {
                return user;
            }
        }

        public List<Account> GetAllUserAccounts(string userId)
        {
            List<Account> accounts = new List<Account>();
            UserDetails userDetails = GetUserDetails(userId);
            foreach (string accountId in userDetails.accounts)
            {
                accounts.Add(_account.GetAccount(accountId));

            }
            return accounts;
        }

        private long GenerateUniqueUserId()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (CheckUser("USR"+id));
            return id;

        }
    }
}
