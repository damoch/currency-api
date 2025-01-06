using CurrencyAPI.Data;
using CurrencyAPI.Shared.Abstracts;
using Newtonsoft.Json;

namespace CurrencyAPI.Shared.Implementations
{
    //https://api.nbp.pl/api/exchangerates/rates/a/chf/
    //https://api.nbp.pl/api/exchangerates/rates/a/usd/2016-04-04/?format=json
    //https://api.nbp.pl/api/exchangerates/rates/a/eur/2024-01-01/?format=json
    //{"table":"C","currency":"dolar amerykański","code":"USD","rates":[{"no":"064/C/NBP/2016","effectiveDate":"2016-04-04","bid":3.6929,"ask":3.7675}]}
    public class NPBApiService : IRemoteApiService
    {
        private ILogger<NPBApiService> _logger;

        public NPBApiService(ILogger<NPBApiService> logger)
        {
            _logger = logger;
        }

        public async Task<CurrencyDataDto> DownloadData(DateTime date, string currencyCode)
        {
            CurrencyDataDto result;
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var dateFormatted = date.ToString("yyyy-MM-dd");
                var requestUri = $"https://api.nbp.pl/api/exchangerates/rates/c/{currencyCode}/{dateFormatted}/?format=json";
                var nbpResponse = await client.GetAsync(requestUri);
                if (!nbpResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed NBP API Request {0}, response type {1} - {2}", requestUri, nbpResponse.StatusCode, nbpResponse.ReasonPhrase);
                    return null;
                }
                nbpResponse.EnsureSuccessStatusCode();

                string json = await nbpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ExchangeRateResponse>(json);
                result = response.AsDto(date);
            }
            return result;
        }
    }



    public class ExchangeRateResponse
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<Rate> Rates { get; set; }

        public CurrencyDataDto AsDto(DateTime date)
        {
            return new CurrencyDataDto
            {
                CurrencyCode = Code,
                CurrencyName = Currency,
                Date = date,
                PurchaseRate = Rates[0].Bid,
                SellRate = Rates[0].Ask,
            };
        }
    }

    public class Rate
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }

}
