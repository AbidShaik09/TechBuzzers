using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface IBills
    {
        public List<Bill> generateRandomBills();

        public Bill generateBill(BillDetails bill);
        public BillDetails getBillDetails(string billDetailsId);
        public bool checkBill(string billId);
        public bool checkBillDetails(string billDetailsId);
        public Bill payBill(Bill bill, Account a);
    }
}
