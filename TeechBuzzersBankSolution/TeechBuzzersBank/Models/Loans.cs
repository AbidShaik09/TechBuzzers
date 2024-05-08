using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techbuzzers_bank.Models
{
    public class Loans
    {
        [Key]
      
        public string Id { get; set; }

        [Required(ErrorMessage = "Account ID is required")]

        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }

        [Required(ErrorMessage = "Loan type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Timestamp is required")]
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Loan amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Loan amount must be greater than zero")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "Rate of interest is required")]
        [Range(0.01, 100, ErrorMessage = "Rate of interest must be between 0.01 and 100")]
        public int Roi { get; set; }

        [Required(ErrorMessage = "Tenure is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Tenure must be at least 1")]
        public int Tenure { get; set; }

        [Required(ErrorMessage = "Due amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Due amount must be greater than zero")]
        public float Due { get; set; }
        [NotMapped]
        public List<string> Payables { get; set; } = new List<string>();

        public string Status { get; set; } = "Open";
    }
}
