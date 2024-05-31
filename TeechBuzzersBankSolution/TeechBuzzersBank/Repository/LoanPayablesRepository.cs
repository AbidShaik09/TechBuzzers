using Microsoft.EntityFrameworkCore;
using System;
using Techbuzzers_bank.Data;
using Techbuzzers_bank.Models;
using Techbuzzers_bank.Repository;
using TeechBuzzersBank.Interface;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Repository
{
    public class LoanPayablesRepository:ILoanPayables
    {

        private readonly ApplicationDbContext _db;
        private TransactionRepository _transaction;
        private AccountRepository _account;

        public LoanPayablesRepository( ApplicationDbContext db)
        {
            _db=db;
            _transaction = new TransactionRepository(db);
            _account = new AccountRepository(db);
        }


        public Transactions payInstallment(LoanPayables loanPayable, Account account)
        {
            if(account.Balance< loanPayable.Amount)
            {
                throw new Exception("Insuffecient Balance!");
            }

            //get admin account
            Account adminAccount = _account.GetAccount("ACN42833749");
            try
            {

                loanPayable.transaction = _transaction.transfer(account, adminAccount, loanPayable.Amount, "Loan Installment");
                
                loanPayable.Status = "Paid";
                _db.SaveChanges();
                return loanPayable.transaction;

            }
            catch
            {
                throw;
            }

        }


        public LoanPayables getLoanPayable(string loanPayableId)
        {
            return _db.payables.Find(loanPayableId);
        }


        public AllLoanPayables getAllPayables(string userId)
        {
            UserDetails user = _db.userDetails.Include(u => u.loans).ThenInclude(w => w.Payables).FirstOrDefault(e => e.Id == userId);
            AllLoanPayables payables = new AllLoanPayables();
            foreach (Loans l in user.loans)
            {
                List<LoanPayables> p = l.Payables;
                foreach (LoanPayables loanPay in p)
                {
                    if (loanPay.Status.Equals("Paid"))
                    {
                        payables.paid.Add(loanPay);      
                    }
                    else if (loanPay.dueDate < DateTime.UtcNow)
                    {
                        loanPay.Status = "Due";
                        payables.due.Add(loanPay);

                    }
                    else
                    {
                        payables.pending.Add(loanPay);
                    }
                }
            }
            SortAscendingLoanPayables sap = new SortAscendingLoanPayables();
            payables.paid.Sort(sap);
            payables.pending.Sort(sap);
            payables.due.Sort(sap);
            _db.SaveChanges();
            return payables;
        }

        public List<LoanPayables> getUpcomingPayables(string userId)
        {
            UserDetails user= _db.userDetails.Include(u=>u.loans).ThenInclude(w=>w.Payables).ThenInclude(E=>E.transaction).FirstOrDefault(e=> e.Id==userId);
            List<LoanPayables> payables = new List<LoanPayables>();
            foreach(Loans l in user.loans)
            {
                List<LoanPayables> p = l.Payables;
                foreach (LoanPayables loanPay in p)
                {

                    if(!loanPay.Status.Equals("Paid") && loanPay.dueDate < DateTime.UtcNow)
                    {
                        loanPay.Status = "Due";
                    }
                    //if(loanPay.dueDate<= DateTime.UtcNow )
                    if (loanPay.dueDate <= DateTime.UtcNow.AddYears(3) )
                        payables.Add(loanPay);
                }
                
            }
            SortAscendingLoanPayables sap = new SortAscendingLoanPayables();
            payables.Sort(sap);
            _db.SaveChanges();
            return payables;
             
        }

        private class SortAscendingLoanPayables : IComparer<LoanPayables>
        {
            int IComparer<LoanPayables>.Compare(LoanPayables a, LoanPayables b)
            {
                if (a.dueDate < b.dueDate) return -1;
                if (a.dueDate > b.dueDate) return 1;
                return 0;
            }
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
