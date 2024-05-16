using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

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
                UserDetails user = _db.userDetails.FirstOrDefault(e=>e.Id==id);
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
            catch (Exception ex)
            {
                    throw new Exception("The user being updated has been deleted by another user.");
              
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
                    foreach (var account in user.accounts)
                    {
                        string accountId=account.Id;
                        _account.DeleteAccount(accountId);
                        user.accounts.Remove(account);
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

        public bool CheckUser(long phone)
        {
            return _db.userDetails.Any(e => e.PhoneNumber == phone);
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
            accounts = _db.account.Where(e=>e.UserId==userId).ToList(); 

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

        public string getIdFromToken(string Token)
        {
            // Retrieve the JWT token from the HTTP request headers
            var token = Token;

            // Parse the JWT token to extract the claims
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Find the claim with the name "UserId" and extract its value
            var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");

            if (userIdClaim == null)
            {
                throw new Exception("UserId claim not found in the JWT token.");
            }

            var userId = userIdClaim.Value;
            return userId;
        }

        public PublicUserDetails getPublicDetails(UserDetails user,long phone)
        {
            PublicUserDetails publicUser = new PublicUserDetails();
            
            UserDetails otherUser = _db.userDetails.FirstOrDefault(e=>e.PhoneNumber==phone);
            if (otherUser == null)
            {
                throw new Exception("User Not Found");
            }
            publicUser.userId = otherUser.Id;
            publicUser.transactions = new List<Transactions>();
            publicUser.phoneNumber = phone;
            publicUser.name = otherUser.FirstName + " " + otherUser.LastName;
            publicUser.primaryAccountId = otherUser.PrimaryAccountId;
            
            

            return publicUser;
        }

        public void AddLoanToUser(Loans loanData,string userId)
        {
            UserDetails user = _db.userDetails.Find(userId);
            if(user == null)
            {
                throw new Exception("User Not Found");
            }
            user.loans.Add(loanData);
            
        }

    }
}
