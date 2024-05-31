
namespace TeechBuzzersBank.Models
{
    public class AllInsurancePayables
    {
        public List<InsurancePayables> pending { get; set; } = new List<InsurancePayables>();
        public List<InsurancePayables> due { get; set; } = new List<InsurancePayables>();
        public List<InsurancePayables> paid { get; set; } = new List<InsurancePayables>();

    }
}
