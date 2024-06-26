﻿using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class LoansResultFormat
    {


        public string loanId { get; set; }
        public string loanType { get; set; }
        public DateTime timeStamp { get; set; }
        public string loanDetailsId { get; set; }

        public string AccountId { get; set; }

        public float LoanAmount { get; set; }
        public int Tenure {  get; set; }
        public LoansResultFormat(Loans l)
        {


            loanId = l.Id;
            loanDetailsId=l.loanDetailsId;
            AccountId = l.AccountId;
            LoanAmount = l.LoanAmount;
            Tenure= l.Tenure;
            timeStamp= l.Timestamp;
            
        }

    }
}
