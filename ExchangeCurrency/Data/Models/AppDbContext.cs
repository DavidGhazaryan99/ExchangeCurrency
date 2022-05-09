using Microsoft.EntityFrameworkCore;

namespace ExchangeCurrency.Data.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }
        public DbSet<Currency_Symbol> Currency_Symbol { get; set; }

    }
}
