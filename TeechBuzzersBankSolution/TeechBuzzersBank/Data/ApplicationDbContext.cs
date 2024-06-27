using Microsoft.EntityFrameworkCore;
using Techbuzzers_bank.Models;
using TeechBuzzersBank.Models;

namespace Techbuzzers_bank.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) {


        }
        public DbSet<UserDetails> userDetails { get; set; }

        public DbSet<Account> account { get; set; }

        public DbSet<Transactions> transactions { get; set; }

        public DbSet<Loans> loans { get; set; }
        public DbSet<LoanPayables> payables { get; set; }
        public DbSet<LoanDetails> loanDetails { get; set; }
        public DbSet<BillDetails> billDetails { get; set; }
        public DbSet<Bill> bill { get; set; }
        public DbSet<InsurancePolicies> insurancePolicies { get; set;}
        public DbSet<Insurance> insurance {  get; set; }
        public DbSet<InsurancePayables> insurancePayables { get; set; } 
        public DbSet<LoanRequest> loanRequests { get; set; }
        public DbSet<InsuranceRequest> insuranceRequests { get; set; }
    }
}
