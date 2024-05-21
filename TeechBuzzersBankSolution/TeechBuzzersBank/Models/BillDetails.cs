using System.ComponentModel.DataAnnotations;

namespace TeechBuzzersBank.Models
{
    public class BillDetails
    {
        [Key]
        public string BillId { get; set;}
        public string BillType { get; set;}
        public string BillingAccount { get; set;}
        public long BillingAccountPhoneNumber {  get; set;}
        public string BillProviderName { get; set; }

    }
}
