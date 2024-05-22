using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Repository
{
    public class InsuranceRepository:IInsurance
    {
        private readonly ApplicationDbContext _db;
        private readonly IUsers _user;
        private readonly ITransaction _transaction;
        private readonly IAccount _account;
        private readonly IInsurancePayables _Ipayable;
        public InsuranceRepository(ApplicationDbContext db)
        {
            _db = db;
            _user = new UserRepository(db);
            _account = new AccountRepository(db);
            _transaction = new TransactionRepository(db);
            _Ipayable = new InsurancePayablesRepository(db);

        }
        public List<InsurancePolicies> getInsurancePolicies()
        {
            return _db.insurancePolicies.ToList();

        }
        public InsurancePolicies getInsurancePolicy(string insurancePolicyId)
        {
            return _db.insurancePolicies.Find(insurancePolicyId);
        }

        public DateTime getInsuranceValidity(string insuranceId)
        {
            return (DateTime) _db.insurance.FirstOrDefault(e => e.id == insuranceId).valididTill;
        }

        public List<Insurance> GetUserInsurances(string userId)
        {
            return _db.insurance.Where(e=>e.UserDetailsId == userId).ToList();
        }
        public Insurance GetInsurance(string insuranceId)
        {
            return _db.insurance.Find(insuranceId);
        }

        public Insurance ApplyInsurance(Insurance insurance, Account account)
        {
            insurance.id = "INO"+generateUniqueInsuranceNumber();
            insurance = calculateAmountCovered(insurance);
            insurance.valididTill = DateTime.UtcNow.AddYears(getInsurancePolicy(insurance.insurancePolicyId).Insurancevalidity);
            insurance.claimed = false;
            Account bankAccount = _account.GetAccount("ACN42833749");
            insurance.payables = new List<InsurancePayables>();

            DateTime dateTime = DateTime.UtcNow;
            for(int i=0;i< getInsurancePolicy(insurance.insurancePolicyId).Insurancevalidity; i++)
            {
                insurance = _Ipayable.generateInsurancePayables(insurance, dateTime,i);
                   
            }
            //now pay first payable then save changes and return insurance


            
        }

        public Insurance calculateAmountCovered(Insurance insurance)
        {
            insurance.amountCovered=(float)((insurance.yearOfPurchase/(DateTime.Now.Year))*insurance.purchaseAmount);
            insurance.installmentAmount=insurance.amountCovered/(getInsurancePolicy(insurance.insurancePolicyId).Insurancevalidity);
            return insurance;
        }

        public bool checkInsurance(string insuranceId)
        {
            return null != _db.insurance.Find(insuranceId);
        }
        public bool checkInsurancePolicy(string insurancePolicyId)
        {
            return null != _db.insurancePolicies.Find(insurancePolicyId);
        }


        public long generateUniqueInsuranceNumber()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (checkInsurance("INO" + id));
            return id;
        }
    }
}
