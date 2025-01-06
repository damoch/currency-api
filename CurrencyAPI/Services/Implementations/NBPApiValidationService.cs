using CurrencyAPI.Services.Abstracts;
using Nager.Holiday;

namespace CurrencyAPI.Services.Implementations
{
    public class NBPApiValidationService : IValidationSevice
    {
        private static readonly string[] AvailableCurrencies = new[]
        {
            "EUR", "USD", "RBL", "BTC", "CHF"
        };

        private static DateTime OldestPossibleDate => new DateTime(2002, 1, 2);//NBP nie udostepnia danych starszych od tej daty

        public bool IsHoliday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public bool IsValidCurrencyCode(string code)
        {
            return string.IsNullOrWhiteSpace(code) || AvailableCurrencies.Contains(code.ToUpper());
        }

        public bool ValidateDate(DateTime date)
        {
            return date >= OldestPossibleDate && date <= DateTime.Now;
        }
    }
}
