using CurrencyAPI.Data;

namespace CurrencyAPI.Shared.Abstracts
{
    public interface INPBApiService
    {
        Task<CurrencyDataDto> DownloadData(DateTime date, string currencyCode);
    }
}
