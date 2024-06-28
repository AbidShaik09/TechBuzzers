using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using TeechBuzzersBank.Models;

namespace Techbuzzers_bank.Models
{
    public class Loans
    {
        [Key]
        [JsonIgnore]
        public string Id { get; set; } = "23621843";

        [Required]
        public string loanDetailsId { get; set; }

        [Required(ErrorMessage = "Account ID is required")]

        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }
        [JsonIgnore]
        public string LoanType { get; set; } = "Personal Loan";
        [JsonIgnore]
        public DateTime Timestamp { get; set; }= DateTime.UtcNow;

        [Required(ErrorMessage = "Loan amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Loan amount must be greater than zero")]
        public float LoanAmount { get; set; }


        [Required]
        public int Tenure { get; set; }
        [JsonIgnore]
        public float TenureAmount { get; set;}
        [JsonIgnore]
        public float Due { get; set; }

        [JsonIgnore]
        public int paidTenures { get; set; } = 0;
        [JsonIgnore]
        [AllowNull]
        public List<Transactions>? paidTenuresList { get; set; } 

        [JsonIgnore]
        public List<LoanPayables>? Payables { get; set; } = new List<LoanPayables>();


        [AllowNull]
        public string? Status { get; set; } = "Active";

        
    }
}
