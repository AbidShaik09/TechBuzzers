using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            return _db.insurance.Include(e=>e.payables).FirstOrDefault(e=>e.id.Equals( insuranceId));
        }

        public Insurance ApplyInsurance(Insurance insurance, Account account)
        {
            
            
            returnBody b=calculateAmountCovered(insurance);

            insurance.amountCovered = b.amountCovered;
            insurance.installmentAmount = b.installMentAmount;
            insurance.valididTill = DateTime.UtcNow.AddYears(getInsurancePolicy(insurance.insurancePolicyId).Insurancevalidity);
            insurance.claimed = false;
            Account bankAccount = _account.GetAccount("ACN42833749");
            insurance.payables = new List<InsurancePayables>();
            Account userAccount = _account.GetAccount(account.Id);
            DateTime dateTime = DateTime.UtcNow;
            for(int i=0;i< getInsurancePolicy(insurance.insurancePolicyId).Insurancevalidity; i++)
            {
                insurance = _Ipayable.generateInsurancePayables(insurance, dateTime,i);
                dateTime= dateTime.AddYears(1);
            }
            //now pay first payable then save changes and return insurance
            insurance.status = "Active";
            _db.SaveChanges();
            InsurancePayables p1 = insurance.payables.FirstOrDefault(e=>e.InstallmentYear==1);
            Transactions t = _Ipayable.payInstallment(p1,userAccount);
            _db.SaveChanges();
            return insurance;



            
        }
        
        public returnBody calculateAmountCovered(Insurance insurance)
        {
            double p = 0.9+(insurance.yearOfPurchase / 2024)/10;
            
                
                insurance.amountCovered = (float)( p * insurance.purchaseAmount);
            insurance.installmentAmount=insurance.amountCovered/(getInsurancePolicy(insurance.insurancePolicyId).Insurancevalidity) / 3;

            insurance.valididTill = DateTime.Now.AddYears( getInsurancePolicy(insurance.insurancePolicyId).Insurancevalidity);
            returnBody returnBody = new returnBody();
            returnBody.installMentAmount =(float) insurance.installmentAmount;
            returnBody.amountCovered = (float)insurance.amountCovered;

            return returnBody;
        }

        public bool checkInsurance(string insuranceId)
        {
            return null != _db.insurance.Find(insuranceId);
        }
        public bool checkInsurancePolicy(string insurancePolicyId)
        {
            return null != _db.insurancePolicies.Find(insurancePolicyId);
        }
        public Transactions claimInsurance(string insuranceId,Account account)
        {
            Insurance i = GetInsurance(insuranceId);
            Account BankAccount = _account.GetAccount("ACN42833749");
            BankAccount.Balance +=(double) i.amountCovered;
            Transactions t = _transaction.transfer(BankAccount,account,(float)i.amountCovered,"Insurance Claim");
            
            i.claimed = true;
            foreach(InsurancePayables ip in i.payables)
            {
                ip.Status = "Claimed";
            }
            _db.SaveChanges();
            return t;


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
