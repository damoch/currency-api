using CurrencyAPI.Services.Abstracts;
using Nager.Holiday;

namespace CurrencyAPI.Services.Implementations
{
    public class NBPApiValidationService : IValidationSevice
    {
        public NBPApiValidationService(HolidayClient holidayClient) 
        {
            _holidayClient = holidayClient;
        }

        private static readonly string[] AvailableCurrencies = new[]
        {
            "EUR", "USD", "RBL", "BTC", "CHF"
        };
        private HolidayClient _holidayClient;

        private static DateTime OldestPossibleDate => new DateTime(2002, 1, 2);//NBP nie udostepnia danych starszych od tej daty

        public async Task<bool> IsHoliday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            return (await _holidayClient.GetHolidaysAsync(date.Year, "pl")).Any(x => x.Date == date);
        }

        public bool IsValidCurrencyCode(string code)
        {
            return AvailableCurrencies.Contains(code.ToUpper());
        }

        public bool ValidateDate(DateTime date)
        {
            return date >= OldestPossibleDate && date <= DateTime.Now;
        }
    }
}
