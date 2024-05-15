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
        public DbSet<Payables> payables { get; set; }
        public DbSet<LoanDetails> loanDetails { get; set; }
    }
}
