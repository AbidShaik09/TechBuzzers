using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class InsurancePayables
    {

        [Key]
        public string Id { get; set; }
        [AllowNull]
        [JsonIgnore]
        public Transactions transaction { get; set; }



        [ForeignKey(nameof(Insurance))]
        public string InsuranceId { get; set; }


        [Required(ErrorMessage = "Month is required")]
        [Range(1, 1200, ErrorMessage = "Month must be between 1 and 1200")]
        public int InstallmentYear { get; set; }


        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public float InstallmentAmount { get; set; }

        public DateTime dueDate { get; set; }

        public string Status { get; set; } = "Pending"; // Due/ Done / Pending
    }
}
