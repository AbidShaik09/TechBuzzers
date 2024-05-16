using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techbuzzers_bank.Models
{
    public class LoanPayables
    {
        [Key]
        public string Id { get; set; }

        

        [ForeignKey(nameof(Loans))]
        public string LoanId { get; set; }


        [Required(ErrorMessage = "Month is required")]
        [Range(1, 1200, ErrorMessage = "Month must be between 1 and 1200")]
        public int Month { get; set; }


        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public float Amount { get; set; }
          
        public DateTime dueDate { get; set; }

        public string Status { get; set; } = "Pending"; // Due/ Done / Pending
    }
}
