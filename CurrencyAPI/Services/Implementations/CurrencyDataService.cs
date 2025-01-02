using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;
using CurrencyAPI.Shared.Abstracts;

namespace CurrencyAPI.Services.Implementations
{
    public class CurrencyDataService : ICurrencyDataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly INPBApiService _nbpApiService;
        private readonly ILogger<CurrencyDataService> _logger;
        private static readonly string[] AvailableCurrencies = new[]
        {
            "EUR", "USD", "RBL", "BTC", "CHF"
        };

        private static DateTime OldestPossibleDate => new DateTime(2002, 1, 2);//NBP nie udostepnia danych starszych od tej daty

        public CurrencyDataService(ApplicationDbContext dbContext, INPBApiService nbpApiService, ILogger<CurrencyDataService> logger)
        {
            _dbContext = dbContext;
            _nbpApiService = nbpApiService;
            _logger = logger;
        }
        public async Task<CurrencyDataDto> GetCurrencyDataFor(string currencyCode, DateTime date)
        {
            CurrencyDataDto result;
            var instance = _dbContext.CurrencyRates.AsQueryable().FirstOrDefault(x => x.CurrencyCode == currencyCode && x.Date == date);
            if (instance == null)
            {
                var dto = await _nbpApiService.DownloadData(date, currencyCode);

                if(dto == null)
                {
                    //_logger.LogError("Fa")
                    return null;
                }

                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    var x = await _dbContext.CurrencyRates.AddAsync(CurrencyRate.FromDto(dto));
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                result = dto;
            }
            else
            {
                result = instance.AsDto();
            }
            return result;
        }

        public bool ValidateCurrencyCode(string currencyCode)
        {
            return AvailableCurrencies.Contains(currencyCode.ToUpper());
        }

        public bool ValidateDate(DateTime date)
        {
            return date >= OldestPossibleDate && date <= DateTime.Now;
        }
    }
}
