using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class LoanRequest
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Loans))]
        public string loanId { get; set; }

        public string userName {  get; set; }
        
        public string accountId { get; set; }
        public string userId { get; set; }
        public double balance {  get; set; }
        public string loanType { get; set; }
        public double requestedAmount { get; set; }
        public int tenure {  get; set; }

        public string status {  get; set; } 
    }
}
