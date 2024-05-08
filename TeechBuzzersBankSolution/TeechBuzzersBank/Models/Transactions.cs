using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techbuzzers_bank.Models
{
    public class Transactions
    {
        [Key]
       
        public string Id { get; set; }

        [Required(ErrorMessage = "Timestamp is required")]
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public float Amount { get; set; }

        [ForeignKey(nameof(UserDetails))]
        public long DebitId { get; set; }

        [ForeignKey(nameof(UserDetails))]
        public long CreditId { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
