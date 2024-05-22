using System.ComponentModel.DataAnnotations;
using Techbuzzers_bank.Models;

namespace TeechBuzzersBank.Models
{
    public class InsurancePolicies
    {
        [Key]
        public string Id { get; set; }
        public string InsuranceType {  get; set; }
        public string InsuranceAccountId { get; set; }

        public int Insurancevalidity {  get; set; }
        

    }
}
