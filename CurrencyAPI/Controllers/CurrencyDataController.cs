using CurrencyAPI.Data;
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
        private readonly INPBApiService _nbpApiService;
        private readonly ApplicationDbContext _dbContext;

        public CurrencyDataController(ILogger<CurrencyDataController> logger, INPBApiService nBPApiService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _nbpApiService = nBPApiService;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetCurrencyData/{currencyCode}/{date}")]
        public async Task<CurrencyDataDto> Get(string currencyCode, DateTime? date)
        {
            if (!date.HasValue)
            {
                date = DateTime.Today;
            }

            if (date.Value > DateTime.Now)
            {
                throw new ArgumentException("Wyst¹pi³a próba pobrania kursu z przysz³oœci!");
            }

            if (!AvailableCurrencies.Contains(currencyCode))
            {
                throw new ArgumentException("Nieznana waluta!");
            }

            //if not in database...
            return await _nbpApiService.DownloadData(date.Value, currencyCode);

            
            //using (var transaction = _dbContext.Database.BeginTransaction())
            //{

            //}
            //var instacne = _dbContext.CurrencyRates.FirstOrDefault(x => x.CurrencyCode == currencyCode && x.Date == date);
            //if (instacne == null) { 
            //}
            //var result =  instacne.AsDto();


            throw new NotImplementedException();
        }
    }
}
