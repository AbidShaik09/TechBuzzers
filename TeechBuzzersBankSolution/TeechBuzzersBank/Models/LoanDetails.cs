using System.ComponentModel.DataAnnotations;

namespace TeechBuzzersBank.Models
{
    public class LoanDetails
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string LoanType { get; set; }
        [Required]
        public float ROI { get; set; }
        [Required]
        public int LoanTenure { get; set;}
        [Required]
        public double AmouuntGranted { get; set; }

    }
}
