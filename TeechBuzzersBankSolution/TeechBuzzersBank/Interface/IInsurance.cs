using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface IInsurance
    {
        public List<InsurancePolicies> getInsurancePolicies();
        public InsurancePolicies getInsurancePolicy(string insurancePolicyId);
        public long generateUniqueInsuranceNumber();
        public DateTime getInsuranceValidity(string insuranceId);

        public List<Insurance> GetUserInsurances(string userId);
        public Insurance GetInsurance(string insuranceId);

        public Insurance ApplyInsurance(Insurance insurance, Account account);

        public returnBody calculateAmountCovered(Insurance insurance);


        public bool checkInsurance(string insuranceId);
        public bool checkInsurancePolicy(string insurancePolicyId);
        public Transactions claimInsurance(string insuranceId, Account account);
     
    }
}
