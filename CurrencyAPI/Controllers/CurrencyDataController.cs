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

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = AvailableCurrencies[Random.Shared.Next(AvailableCurrencies.Length)]
            })
            .ToArray();
        }
    }
}
