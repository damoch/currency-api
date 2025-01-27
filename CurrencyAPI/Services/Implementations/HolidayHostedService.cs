
using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;
using Nager.Holiday;

namespace CurrencyAPI.Services.Implementations
{
    public class HolidayHostedService : IHostedService
    {
        private IServiceProvider _serviceProvider;

        public HolidayHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var currentYear = DateTime.Now.Year;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (dbContext.HolidayDates.Any(x => x.Date.Year == currentYear))
                {
                    return;
                }
                var validationService = scope.ServiceProvider.GetRequiredService<IValidationSevice>();
                var holidayClient = scope.ServiceProvider.GetRequiredService<HolidayClient>();
                for (int i = validationService.OldestPossibleDate.Year; i <= currentYear; i++)
                {
                    if (dbContext.HolidayDates.Any(x => x.Date.Year == i))
                    {
                        continue;
                    }
                    try
                    {
                        var holidays = await holidayClient.GetHolidaysAsync(i, "PL");
                        foreach (var holiday in holidays)
                        {
                            dbContext.HolidayDates.Add(new HolidayDate
                            {
                                Date = holiday.Date,
                                Name = holiday.LocalName
                            });
                        }
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //_logger.LogError(ex, $"An error occured while fetching holidays for year {i}: {ex}");
                        continue;
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
