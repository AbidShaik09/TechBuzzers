using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface ITransaction
    {
        public Transactions transfer(Account senderAcc,Account receiverAcc,float amount, string transactionType = "UserToUser Transfer");
        public string getTransactionStatus(string transactionId);
        public List<Transactions> getTransactions(string currentUserId,string oppositeUserId);
        public List<Transactions> getTransactions(string userId);
        public List<Transactions> getAccountTransactions(string accId);
        public Transactions getTransaction(string transactionId);

        public bool CheckTransaction(string transactionId);

    }
}
