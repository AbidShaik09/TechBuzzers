using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface IBills
    {

        public BillDetails getBillDetails(string billDetailsId);
        public long GenerateUniqueBillId();
        public List<Bill> getRecentBillPayments(string userId);
        public List<BillDetails> getBillDetails();
        public bool checkBill(string billId);
        public bool checkBillDetails(string billDetailsId);
        public Bill payBill(Bill bill, Account a);
    }
}
