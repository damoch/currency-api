using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;
using CurrencyAPI.Shared.Abstracts;
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
        private readonly ICurrencyDataService _currencyService;
        private readonly ApplicationDbContext _dbContext;

        public CurrencyDataController(ILogger<CurrencyDataController> logger, ICurrencyDataService currencyService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _currencyService = currencyService;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetCurrencyData/{currencyCode}/{date}")]
        public async Task<IActionResult> Get(string currencyCode, DateTime? date)
        {
            if (!date.HasValue)
            {
                date = DateTime.Today;
            }

            if (date.Value > DateTime.Now)
            {
                NotFound("Wyst¹pi³a próba pobrania kursu z przysz³oœci!");
            }
            currencyCode = currencyCode.ToUpper();
            if (!AvailableCurrencies.Contains(currencyCode))
            {
                NotFound("Nieznana waluta!");
            }

            CurrencyDataDto result = await _currencyService.GetCurrencyDataFor(currencyCode, date.Value);
            return Ok(result);
        }
    }
}
