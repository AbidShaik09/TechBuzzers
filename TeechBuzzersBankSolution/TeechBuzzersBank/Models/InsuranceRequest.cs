using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class InsuranceRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Insurance))]
        public string insuranceId { get; set; }

        public string userName { get; set; }
        
        public string accountId { get; set; }
        public string userId { get; set; }
        public double balance { get; set; }
        public string insuranceType { get; set; }

        public string uniqueIdentificationNumber {  get; set; }
        public int yearOfPurchase { get; set; }

        public double purchaseAmount { get; set; }
        public double installmentAmount { get; set; }

        public double amountCovered { get; set; }
        public string status {  get; set; }

    }
}
