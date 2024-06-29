using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class Insurance
    {

        [Key]
        public string id { get; set; } = "SomeId";

        public string insurancePolicyId {  get; set; }
        public string UniqueIdentificationNumber { get; set; }
        public int yearOfPurchase { get; set; }

        [ForeignKey(nameof(UserDetails))]
        public string? UserDetailsId { get; set; }
        public double purchaseAmount { get; set; }

        public DateTime? valididTill { get; set; }
        [JsonIgnore]
        public List<InsurancePayables>? payables { get; set; }
        public float? installmentAmount { get; set; }
        public float? amountCovered { get; set; }
        [JsonIgnore]
        public bool claimed { get; set; } = false;

        [AllowNull]
        public string? status { get; set; } 

        


    }
}
