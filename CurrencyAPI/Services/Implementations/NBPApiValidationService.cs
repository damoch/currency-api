using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;

namespace CurrencyAPI.Services.Implementations
{
    public class NBPApiValidationService : IValidationSevice
    {
        private ApplicationDbContext _dbContext;

        public NBPApiValidationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string[] AvailableCurrencies =>
        [
            "EUR", "USD", "SEK", "CZK", "CHF", "GBP", "CAD", "AUD", "HUF", "JPY", "DKK", "NOK", "XDR"
        ];

        public DateTime OldestPossibleDate => new DateTime(2002, 1, 2);//NBP nie udostepnia danych starszych od tej daty

        public bool IsHoliday(DateTime date)
        {
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            if (_dbContext.HolidayDates.Any(h => h.Date == date))
            {
                return true;
            }
            return false;
        }

        public bool IsValidCurrencyCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && AvailableCurrencies.Contains(code.ToUpper());
        }

        //12.25.2024
        public bool ValidateDate(DateTime date)
        {
            return date >= OldestPossibleDate && date <= DateTime.Now;
        }
    }
}
