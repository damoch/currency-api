using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyDataController : ControllerBase
    {
        private static readonly string _baseCurrency = "PLN";
        private static readonly string[] AvailableCurrencies = new[]
        {
            "EUR", "USD", "RBL", "BTC", "CHF"
        };

        private readonly ILogger<CurrencyDataController> _logger;

        public CurrencyDataController(ILogger<CurrencyDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCurrencyData/{currencyCode}/{date}")]
        public IEnumerable<CurrencyData> Get(string currencyCode, DateTime? date)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }

            if (date.Value > DateTime.Now) {
                throw new ArgumentException("Wyst¹pi³a próba pobrania kursu z przysz³oœci!");
            }

            if (!AvailableCurrencies.Contains(currencyCode)) {
                throw new ArgumentException("Nieznana waluta!");
            }

            throw new NotImplementedException();
        }
    }
}
