using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class Bill
    {
        [Key]
        [JsonIgnore]
        public string Id { get; set; } = "";
        public string billDetailsId {  get; set; }
        public string billType { get; set; }
        public string billNumber { get; set; }
        public float amount { get; set; }

        public Transactions? transaction { get; set; }
    }
}
