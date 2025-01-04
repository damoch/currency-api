using CurrencyAPI.Data;

namespace CurrencyAPI.Shared.Abstracts
{
    public interface IRemoteApiService
    {
        Task<CurrencyDataDto> DownloadData(DateTime date, string currencyCode);
    }
}
