using CurrencyAPI.Data;
using CurrencyAPI.Shared.Abstracts;
using Newtonsoft.Json;

namespace CurrencyAPI.Shared.Implementations
{
    public class NPBApiService : IRemoteApiService
    {
        private ILogger<NPBApiService> _logger;

        public NPBApiService(ILogger<NPBApiService> logger)
        {
            _logger = logger;
        }

        public async Task<CurrencyDataDto> DownloadData(DateTime date, string currencyCode)
        {
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

                string json = await nbpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ExchangeRateResponse>(json);
                return response.AsDto(date);
            }
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
