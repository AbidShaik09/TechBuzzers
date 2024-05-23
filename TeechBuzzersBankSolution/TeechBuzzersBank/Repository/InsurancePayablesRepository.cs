using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Interface;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Repository
{
    public class InsurancePayablesRepository: IInsurancePayables
    {
        private readonly ApplicationDbContext _db;
        private readonly ITransaction _transaction;
        private readonly IAccount _account;
        public InsurancePayablesRepository( ApplicationDbContext db)
        {
            _db = db;
            _transaction = new TransactionRepository(db);
            _account = new AccountRepository(db);
        }

        public List<InsurancePayables> getUpcomingPayables(string userId)
        {

            List<Insurance> insurances= _db.insurance.Include(e=>e.payables).Where(e=>e.UserDetailsId == userId ).ToList();
            List<InsurancePayables> insurancePayables = new List<InsurancePayables>();
            foreach(Insurance i in insurances)
            {
                List<InsurancePayables> insurancepay = _db.insurancePayables.Where(e=>e.dueDate< DateTime.Now.AddMonths(6)).ToList();
                foreach(InsurancePayables ip in insurancepay)
                {
                    if(ip.dueDate<DateTime.UtcNow.AddMonths(6).AddDays(1) && !ip.Status.Equals("Claimed") && !ip.Status.Equals("Paid"))
                        insurancePayables.Add(ip);
                }
            }
            return insurancePayables;
        }

        public Transactions payInstallment(InsurancePayables insurancePayable, Account account)
        {
            if (account.Balance < insurancePayable.InstallmentAmount)
            {
                throw new Exception("Insuffecient Balance!");
            }


            Account adminAccount = _account.GetAccount("ACN42833749");
            try
            {

                insurancePayable.transaction = _transaction.transfer(account, adminAccount, insurancePayable.InstallmentAmount, "Insurance Installment");

                insurancePayable.Status = "Paid";
                _db.SaveChanges();
                return insurancePayable.transaction;

            }
            catch
            {
                throw;
            }
        }

        public Insurance generateInsurancePayables(Insurance insuranceData, DateTime dateTime, int i)
        {
             InsurancePayables p = new InsurancePayables();
            p.Id = "IIN" + GenerateUniqueInsuranceInstallmentId();

            p.InsuranceId = insuranceData.id;
            p.dueDate = dateTime;
            dateTime.AddYears(1);
            p.InstallmentAmount = (float)insuranceData.installmentAmount;
            p.Status = "Pending";
            p.InstallmentYear = i + 1;
            insuranceData.payables.Add(p);
            return insuranceData;
        }
        private long GenerateUniqueInsuranceInstallmentId()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (checkPayables("IIN" + id));
            return id;

        }

        public bool checkPayables(string payablesId)
        {
            return _db.insurancePayables.Find(payablesId)!= null;
        }

        public InsurancePayables getInsurancePayable(string insurancePayablesId)
        {
            return _db.insurancePayables.Find(insurancePayablesId);
        }

    }
}
