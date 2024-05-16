using Microsoft.EntityFrameworkCore;
using System;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;

namespace TeechBuzzersBank.Repository
{
    public class LoanPayablesRepository:ILoanPayables
    {

        private readonly ApplicationDbContext _db;

        public LoanPayablesRepository( ApplicationDbContext db)
        {
            _db=db;

        }
        public List<LoanPayables> getUpcomingPayables(string userId)
        {
            UserDetails user= _db.userDetails.Include(u=>u.loans).ThenInclude(w=>w.Payables).FirstOrDefault(e=>e.Id==userId);
            List<LoanPayables> payables = new List<LoanPayables>();
            foreach(Loans l in user.loans)
            {
                List<LoanPayables> p = l.Payables;
                foreach (LoanPayables loanPay in p)
                {
                    if(loanPay.dueDate<= DateTime.UtcNow.AddMonths(1).AddDays(1) && loanPay.Status!="Paid")
                        payables.Add(loanPay);
                }
            }
            return payables;
             
        }

        public bool checkPayables(string payablesId)
        {
            return _db.payables.Find(payablesId)!= null;    
        }
        private long GenerateUniqueLoanId()
        {
            Random r = new Random();
            long id;
            do
            {
                id = r.Next(10000000, 99999999);

            } while (checkPayables("PBL" + id));
            return id;

        }

        public Loans generateLoanPayables(Loans loanData, DateTime dateTime,int i)
        {
            LoanPayables p = new LoanPayables();
            p.Id = "PBL" + GenerateUniqueLoanId();
            p.LoanId = loanData.Id;
            p.dueDate = dateTime;
            dateTime.AddMonths(1);
            p.Amount = loanData.TenureAmount;
            p.Status = "Pending";
            p.Month = i + 1;
            loanData.Payables.Add(p);
            return loanData;
        }

    }
}
