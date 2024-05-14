using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Techbuzzers_bank.Models
{
    public class Account
    {
        [Key]
       
        public string Id { get; set; }
        [Required]
        public string accountName { get; set; }
        [JsonIgnore]
        public bool isPrimary { get; set; }

        [ForeignKey(nameof(UserDetails))]
        public string UserId { get; set; }



        [Required(ErrorMessage = "Balance is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative number")]
        public float Balance { get; set; }
        
        [NotMapped]
        [JsonIgnore]
        public List<string>? Transactions { get; set; } = new List<string>();
        
        [NotMapped]
        [JsonIgnore]
        public List<string>? Loans { get; set; } = new List<string>();
    }
}
