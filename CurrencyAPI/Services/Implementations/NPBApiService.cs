using Azure;
using CurrencyAPI.Data;
using CurrencyAPI.Shared.Abstracts;
using Newtonsoft.Json;

namespace CurrencyAPI.Shared.Implementations
{
    //https://api.nbp.pl/api/exchangerates/rates/a/chf/
    //https://api.nbp.pl/api/exchangerates/rates/c/usd/2016-04-04/?format=json
    public class NPBApiService : INPBApiService
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
                var requestUri = $"https://api.nbp.pl/api/exchangerates/rates/a/{currencyCode}";//zmienic zapytanie!!!!
                var nbpResponse = await client.GetAsync(requestUri);
                if (!nbpResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed NBP API Request {0}", requestUri);
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

        internal CurrencyDataDto AsDto(DateTime date)
        {
            return new CurrencyDataDto()
            {
                Date = date,
                CurrencyCode = Code,
                PurchaseRate = Rates[0].Mid
            };

        }
    }

    public class Rate
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }

}
