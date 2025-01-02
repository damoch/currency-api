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
        private readonly ILogger<CurrencyDataController> _logger;
        private readonly ICurrencyDataService _currencyService;

        public CurrencyDataController(ILogger<CurrencyDataController> logger, ICurrencyDataService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        [HttpGet(Name = "GetCurrencyData/{currencyCode}/{date}")]
        public async Task<IActionResult> Get(string currencyCode, DateTime? date)
        {
            if (!_currencyService.ValidateCurrencyCode(currencyCode))
            {
                return BadRequest("Nieobslugiwana waluta");
            }

            if (!date.HasValue)
            {
                date = DateTime.Today;
            }
            else if (!_currencyService.ValidateDate(date.Value))
            {
                return BadRequest("Podana data znajduje sie poza obslugiwanym zakresem");
            }

            var result = await _currencyService.GetCurrencyDataFor(currencyCode, date.Value);

            if(result == null)
            {
                return BadRequest("Wystapil blad serwera. Prosze sprobowac pozniej.");
            }

            return Ok(result);
        }
    }
}
