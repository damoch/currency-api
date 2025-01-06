using Castle.Core.Logging;
using CurrencyAPI.Data;
using CurrencyAPI.Services.Implementations;
using CurrencyAPI.Shared.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace CurrencyAPITests
{
    public class CurrencyDataServiceTest
    {
        private Mock<IRemoteApiService> _remoteApiMock;
        private Mock<ILogger<CurrencyDataService>> _loggerMock;
        private CurrencyDataService _currencyDataService;
        private ApplicationDbContext _dbContext;


        private static readonly DateTime _existingDate = new DateTime(2024, 11, 2);
        private static readonly DateTime _newDate = new DateTime(2004, 11, 2);
        private readonly CurrencyRate _firstTestEntry = new CurrencyRate()
        {
            Id = 1,
            CurrencyCode = "USD",
            Date = _existingDate,
            CurrencyName = "dolar amerykański",
        };

        private readonly CurrencyRate _secondTestEntry = new CurrencyRate()
        {
            Id = 2,
            CurrencyCode = "EUR",
            Date = _newDate,
            CurrencyName = "euro",
        };

        private readonly CurrencyDataDto _nullResult = null;

        [OneTimeSetUp]
        public void Setup()
        {
            var options  = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

            _dbContext = new ApplicationDbContext(options);

            _dbContext.CurrencyRates.Add(_firstTestEntry);
            var x = _dbContext.SaveChanges();
            

            _remoteApiMock = new Mock<IRemoteApiService>();

            _remoteApiMock.Setup(x => x.DownloadData(_newDate, "EUR")).ReturnsAsync(_secondTestEntry.AsDto());
            _remoteApiMock.Setup(x => x.DownloadData(_newDate, "BAD")).ReturnsAsync(_nullResult);

            _loggerMock = new Mock<ILogger<CurrencyDataService>>();
            _currencyDataService = new CurrencyDataService(_dbContext, _remoteApiMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task TryAdding_EntryExists()
        {
            var result = await _currencyDataService.GetCurrencyDataFor("usd", _existingDate);

            Assert.That(AreEqualCurrencies(result, _firstTestEntry.AsDto()));
            Assert.That(_dbContext.CurrencyRates.AsQueryable().Count() == 1);
        }

        [Test]
        public async Task TryForIncorrectCurrency()
        {
            var result = await _currencyDataService.GetCurrencyDataFor("Bad", _newDate);

            Assert.That(result == null);
        }

        private bool AreEqualCurrencies(CurrencyDataDto A, CurrencyDataDto B)
        {
            return A.Date == B.Date && A.CurrencyName == B.CurrencyName && A.CurrencyCode == B.CurrencyCode;
        }
    }
}
