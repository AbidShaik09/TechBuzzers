using System.Diagnostics;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;

namespace TeechBuzzersBank.Repository
{
    public class TransactionRepository:ITransaction
    {

        private readonly ApplicationDbContext _db;
        private UserRepository _user;
        private AccountRepository _account;

        public TransactionRepository(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);
        }

        public Transactions transfer(Account senderAcc, Account receiverAcc, float amount)
        {

            Transactions t = new Transactions();
            t.Id = "TRN"+GenerateUniqueTransactionId();
            t.Amount = amount;
            t.Timestamp = DateTime.UtcNow;
            t.DebitId = senderAcc.Id;
            t.CreditId = receiverAcc.Id;

            t.CreditUserId = receiverAcc.UserId;
            t.DebitUserId= senderAcc.UserId;

            if (senderAcc == null || receiverAcc == null)
            {
                throw new Exception(" Sender/ Receiver Account is null!");
            }
            if (senderAcc.Balance < amount)
            {
                t.Status = "failed";
                _db.transactions.Add(t);

                senderAcc.Transactions.Add(t.Id);
                _db.SaveChanges();
                throw new Exception("Insuffecient Balance!");
            }

            
            senderAcc.Balance -= amount;
            receiverAcc.Balance += amount;
            t.Status = "completed";
            _db.transactions.Add(t);
            senderAcc.Transactions.Add(t.Id);
            _db.SaveChanges();
            return t;
        }
        public string getTransactionStatus(string transactionId)
        {
            Transactions transaction = getTransaction(transactionId);
            if (transaction == null)
            {
                throw new Exception("Invalid TransactionId!");
            }
            return transaction.Status;
        }
        public List<Transactions> getTransactions(string currentUserId, string oppositeUserId)
        {
            List<Transactions> currentUserTransactions = getTransactions(currentUserId);



            List<Transactions> commonTransactions = new List<Transactions>();
            foreach (Transactions t in currentUserTransactions)
            {
                if (t.CreditUserId == oppositeUserId )
                {
                    commonTransactions.Add(t);
                }

            }
            List<Transactions> oppositeUserTransactions = getTransactions(oppositeUserId);
            foreach (Transactions t in oppositeUserTransactions)
            {
                if (t.CreditUserId == currentUserId)
                {
                    commonTransactions.Add(t);
                }

            }

            return commonTransactions;

        }
        public List<Transactions> getTransactions(string userId)
        {
            List<Transactions> transactions = new List<Transactions>();


            List<Account> accounts= _user.GetAllUserAccounts(userId);

            foreach (Account a in accounts)
            {

                List<Transactions> t = _db.transactions.Where(e=>e.DebitId==a.Id||e.CreditId==a.Id).ToList();
                if (t != null)
                {
                    foreach(Transactions transactions1 in t)
                        transactions.Add(transactions1);
                }
            }
            return transactions;
        }

        public List<Transactions> getAccountTransactions(string accId)
        {

            List<Transactions> transactions = new List<Transactions>();
            Account account = _account.GetAccount(accId);
            List<Transactions> t = _db.transactions.Where(e => e.DebitId == accId || e.CreditId==accId).ToList();
            if (t != null)
            {
                foreach (Transactions transactions1 in t)
                    transactions.Add(transactions1);
            }

            return transactions;

        }
        public Transactions getTransaction(string transactionId)
        {

            return _db.transactions.FirstOrDefault(e=>e.Id==transactionId);
            

        }

        public bool CheckTransaction(string transactionId)
        {
            return _db.transactions.Any(e=>e.Id==transactionId);
        }

        private long GenerateUniqueTransactionId()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (CheckTransaction("TRN" + id));
            return id;

        }
    }
}
