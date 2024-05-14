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
        [Range(0.0001, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public float Amount { get; set; }

        [ForeignKey(nameof(UserDetails))]
        public string DebitId { get; set; }

        [ForeignKey(nameof(UserDetails))]
        public string CreditId { get; set; }

        public string DebitUserId { get; set; }
        public string CreditUserId { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
