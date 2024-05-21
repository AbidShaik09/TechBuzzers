using System.Globalization;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Repository
{
    public class BillRepository:IBills
    {
        private readonly ITransaction _transaction;
        private readonly IAccount _account;
        private readonly ApplicationDbContext _db;
        public BillRepository(ApplicationDbContext db) { 
            _db = db;
            _account = new AccountRepository(db);
            _transaction = new TransactionRepository(db);
        }

        public List<Bill> generateRandomBills()
        {
            List<Bill> bills = new List<Bill>();
            Random random = new Random();
            int n = random.Next(1, 3);
           
            while (n-- >0)
            {
                int amount = random.Next(100, 300);
                Bill b = new Bill();
                b.Id ="BLN"+ GenerateUniqueBillId();
                b.amount = amount;
                b.billDetailsId = "BLL0000"+random.Next(1,3);
                b.billType = getBillDetails(b.billDetailsId).BillType;
                bills.Add(b);
            }
            return bills;


        }


        public Bill generateBill(BillDetails bill)
        {
            Bill b = new Bill();
            b.billType = bill.BillType;
            b.billDetailsId = bill.BillId;
            Random random = new Random();
            b.amount=random.Next(100, 300);
            return b;
        }
        public BillDetails getBillDetails(string billDetailsId)
        {
            return _db.billDetails.Find(billDetailsId);

        }
        private long GenerateUniqueBillId()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (checkBill("BLN" + id));
            return id;

        }

        public Bill payBill(Bill bill, Account a)
        {
            if (a.Balance < bill.amount)
            {
                throw new Exception("Insufficient Balance!");
            }
            BillDetails bd = getBillDetails(bill.billDetailsId);
            if (bd == null)
            {
                throw new Exception("Invalid billId!");

            }
            string billAccountId = bd.BillingAccount;
            Account billAccount = _account.GetAccount(billAccountId);
            bill.transaction = _transaction.transfer(a, billAccount, bill.amount, bd.BillType + " Payment");
            return bill;
        }
        public bool checkBill(string billId){
            return null != _db.bill.Find(billId);
        }
        public bool checkBillDetails(string billDetailsId)
        {

            return null != _db.billDetails.Find(billDetailsId);
        }
    }
}
