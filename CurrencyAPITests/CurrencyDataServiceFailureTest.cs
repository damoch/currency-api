using CurrencyAPI.Data;
using CurrencyAPI.Services.Implementations;
using CurrencyAPI.Shared.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CurrencyAPITests
{
    public class CurrencyDataServiceFailureTest
    {
        private Mock<IRemoteApiService> _remoteApiMock;
        private Mock<ILogger<CurrencyDataService>> _loggerMock;
        private ApplicationDbContext _applicationDbContextMock;
        private CurrencyDataService _currencyDataService;


        private static readonly DateTime _testDate = new DateTime(2014, 11, 11);
        private static readonly string _testCode = "TEST";

        [SetUp]
        public void Setup()
        {
            _remoteApiMock = new Mock<IRemoteApiService>();

            _remoteApiMock.Setup(x => x.DownloadData(_testDate, _testCode)).Throws(new Exception("Failure"));

            _loggerMock = new Mock<ILogger<CurrencyDataService>>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _applicationDbContextMock = new ApplicationDbContext(options);

            _currencyDataService = new CurrencyDataService(_applicationDbContextMock, _remoteApiMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void Teardown() {

            _applicationDbContextMock.Dispose();
        }

        [Test]
        public async Task TestExternalApiFailure()
        {
            var result = await _currencyDataService.GetCurrencyDataFor(_testCode, _testDate);

            Assert.That(result, Is.Null);
        }
    }
}
