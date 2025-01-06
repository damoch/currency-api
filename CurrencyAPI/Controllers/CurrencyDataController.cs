using CurrencyAPI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyDataController : ControllerBase
    {
        private readonly ILogger<CurrencyDataController> _logger;
        private readonly ICurrencyDataService _currencyService;
        private readonly IValidationSevice _validationService;

        public CurrencyDataController(ILogger<CurrencyDataController> logger, ICurrencyDataService currencyService, IValidationSevice validationService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _validationService = validationService;
        }

        [HttpGet(Name = "GetCurrencyData/{currencyCode}/{date}")]
        public async Task<IActionResult> Get(string currencyCode, DateTime? date)
        {
            if (!_validationService.IsValidCurrencyCode(currencyCode))
            {
                return BadRequest("Nieobslugiwana waluta");
            }

            if (!date.HasValue)
            {
                date = DateTime.Today;
            }

            if (!_validationService.ValidateDate(date.Value))
            {
                return NotFound("Podana data znajduje sie poza obslugiwanym zakresem");
            }

            if (_validationService.IsHoliday(date.Value))
            {
                return NotFound("Dzien wolny od pracy - brak danych");
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
