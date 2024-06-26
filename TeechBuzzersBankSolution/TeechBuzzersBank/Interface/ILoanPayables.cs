﻿using System;
using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace TeechBuzzersBank.Interface
{
    public interface ILoanPayables
    {
        public List<LoanPayables> getUpcomingPayables(string userId);

        public AllLoanPayables getAllPayables(string userId);

        public Transactions payInstallment(LoanPayables loanPayable, Account account);
        public Loans generateLoanPayables(Loans loanData,DateTime dateTime,int i);


        public bool checkPayables(string payablesId);

        public LoanPayables getLoanPayable(string loanPayableId);
    }
}
