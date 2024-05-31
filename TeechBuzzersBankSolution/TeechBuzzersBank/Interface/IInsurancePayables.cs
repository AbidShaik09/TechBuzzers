using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface IInsurancePayables
    {

        public List<InsurancePayables> getUpcomingPayables(string userId);
        public AllInsurancePayables getAllInsurancePayables(string userId);

        public Transactions payInstallment(InsurancePayables insurancePayable, Account account);
        public Insurance generateInsurancePayables(Insurance insuranceData, DateTime dateTime, int i);


        public bool checkPayables(string payablesId);

        public InsurancePayables getInsurancePayable(string insurancePayablesId);

    }
}
