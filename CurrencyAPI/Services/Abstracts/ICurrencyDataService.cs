using CurrencyAPI.Data;

namespace CurrencyAPI.Services.Abstracts
{
    public interface ICurrencyDataService
    {
        Task<CurrencyDataDto> GetCurrencyDataFor(string currencyCode, DateTime date);
    }
}
