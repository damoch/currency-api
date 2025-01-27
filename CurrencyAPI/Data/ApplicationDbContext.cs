using Microsoft.EntityFrameworkCore;

namespace CurrencyAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<HolidayDate> HolidayDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CurrencyRate>()
                .HasIndex(cr => new { cr.CurrencyCode, cr.Date })
                .HasDatabaseName("IX_CurrencyCode_Date");

            modelBuilder.Entity<CurrencyRate>()
                .Property(cr => cr.BuyRate)
                .HasColumnType("decimal(18, 6)"); //Większa precyzja potrzebna do dokladnego skladowania danych

            modelBuilder.Entity<CurrencyRate>()
                .Property(cr => cr.SellRate)
                .HasColumnType("decimal(18, 6)");
        }

    }
}
