using Microsoft.EntityFrameworkCore;

namespace CurrencyAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }
    }
}
