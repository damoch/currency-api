using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;
using CurrencyAPI.Shared.Abstracts;

namespace CurrencyAPI.Services.Implementations
{
    public class CurrencyDataService : ICurrencyDataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IRemoteApiService _nbpApiService;
        private readonly ILogger<CurrencyDataService> _logger;

        public CurrencyDataService(ApplicationDbContext dbContext, IRemoteApiService nbpApiService, ILogger<CurrencyDataService> logger)
        {
            _dbContext = dbContext;
            _nbpApiService = nbpApiService;
            _logger = logger;
        }

        public async Task<CurrencyDataDto> GetCurrencyDataFor(string currencyCode, DateTime date)
        {
            currencyCode = currencyCode.ToUpper();
            CurrencyDataDto result;
            var instance = _dbContext.CurrencyRates.AsQueryable().FirstOrDefault(x => x.CurrencyCode == currencyCode && x.Date == date);
            if (instance == null)
            {
                var dto = await _nbpApiService.DownloadData(date, currencyCode);

                if(dto == null)
                {
                    _logger.LogError("Download failed {0} with date {1}", currencyCode, date);
                    return null;
                }
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var x = await _dbContext.CurrencyRates.AddAsync(CurrencyRate.FromDto(dto));
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        transaction.Rollback();
                        return null;
                    }

                }
                result = dto;
            }
            else
            {
                result = instance.AsDto();
            }
            return result;
        }
    }
}
