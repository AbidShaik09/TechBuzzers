using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TeechBuzzersBank.Models;

namespace Techbuzzers_bank.Models
{
    public class Loans
    {
        [Key]
      
        public string Id { get; set; }

        [Required]
        public string loanDetailsId { get; set; }

        [Required(ErrorMessage = "Account ID is required")]

        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }

        [Required(ErrorMessage = "Loan type is required")]
        public string Type { get; set; }
        [JsonIgnore]
        public DateTime Timestamp { get; set; }= DateTime.UtcNow;

        [Required(ErrorMessage = "Loan amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Loan amount must be greater than zero")]
        public float Amount { get; set; }


        [Required(ErrorMessage = "Tenure is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Tenure must be at least 1")]
        public int Tenure { get; set; }

        [Required(ErrorMessage = "Due amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Due amount must be greater than zero")]
        [JsonIgnore]
        public float Due { get; set; }
        [NotMapped]
        [JsonIgnore]
        public List<string>? Payables { get; set; } = new List<string>();


        [JsonIgnore]
        public string Status { get; set; } = "Active";
    }
}
