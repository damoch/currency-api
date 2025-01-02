using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;
using CurrencyAPI.Shared.Abstracts;
using System;
using System.Reflection.Metadata.Ecma335;

namespace CurrencyAPI.Services.Implementations
{
    public class CurrencyDataService : ICurrencyDataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly INPBApiService _nbpApiService;

        public CurrencyDataService(ApplicationDbContext dbContext, INPBApiService nbpApiService)
        {
            _dbContext = dbContext;
            _nbpApiService = nbpApiService;
        }
        public async Task<CurrencyDataDto> GetCurrencyDataFor(string currencyCode, DateTime date)
        {
            CurrencyDataDto result;
            var instance = _dbContext.CurrencyRates.AsQueryable().FirstOrDefault(x => x.CurrencyCode == currencyCode && x.Date == date);
            if (instance == null)
            {
                var dto = await _nbpApiService.DownloadData(date, currencyCode);
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
        
    }
}
