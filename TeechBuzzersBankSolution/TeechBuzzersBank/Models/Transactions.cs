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
        public string DebitUserName { get; set; }
        public string CreditUserName { get; set; }
        public float openingBalance { get; set; } = 0;
        public float closingBalance { get; set; } = 0;

        public string transactionType { get; set; } = "UserToUser Transfer"; //UserToUser Transfer, Bill Payment, Loan Payment, Self Transfer
        public string Status { get; set; } = "Pending";
    }
}
