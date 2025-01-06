using CurrencyAPI.Services.Abstracts;

namespace CurrencyAPI.Services.Implementations
{
    public class NBPApiValidationService : IValidationSevice
    {
        private static readonly string[] AvailableCurrencies =
        [
            "EUR", "USD", "SEK", "CZK", "CHF", "GBP", "CAD", "AUD", "HUF", "JPY", "DKK", "NOK", "XDR"
        ];

        private static DateTime OldestPossibleDate => new DateTime(2002, 1, 2);//NBP nie udostepnia danych starszych od tej daty

        public bool IsHoliday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public bool IsValidCurrencyCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && AvailableCurrencies.Contains(code.ToUpper());
        }

        public bool ValidateDate(DateTime date)
        {
            return date >= OldestPossibleDate && date <= DateTime.Now;
        }
    }
}
