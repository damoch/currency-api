using Microsoft.EntityFrameworkCore;

namespace CurrencyAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CurrencyRate>()
                .HasIndex(cr => new { cr.CurrencyCode, cr.Date })
                .HasDatabaseName("IX_CurrencyCode_Date");
        }

    }
}
