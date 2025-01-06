using CurrencyAPI.Controllers;
using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CurrencyAPITests
{
    [TestFixture]
    public class Tests
    {
        private Mock<ICurrencyDataService> _mockCurrencyDataService;
        private Mock<IValidationSevice> _mockValidationSevice; 
        private Mock<ILogger<CurrencyDataController>> _mockLogger;
        private CurrencyDataController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCurrencyDataService = new Mock<ICurrencyDataService>();
            _mockValidationSevice = new Mock<IValidationSevice>();
            _mockLogger = new Mock<ILogger<CurrencyDataController>>();
            _controller = new CurrencyDataController(_mockLogger.Object, _mockCurrencyDataService.Object, _mockValidationSevice.Object);
        }


        [Test]
        public async Task GetCurrencyData_CorrectAndIncorrectCurrencyCode()
        {
            // Arrange
            string correctCurrencyCode = "EUR";
            string incorrectCurrencyCode = "BAD";
            DateTime? date = DateTime.Today;

            _mockValidationSevice
                .Setup(v => v.IsValidCurrencyCode(correctCurrencyCode))
                .Returns(true);

            _mockValidationSevice
                .Setup(v => v.IsValidCurrencyCode(incorrectCurrencyCode))
                .Returns(false);

            _mockValidationSevice.Setup(v => v.ValidateDate(date.Value)).Returns(true);

            _mockCurrencyDataService.Setup(c => c.GetCurrencyDataFor(correctCurrencyCode, date.Value)).ReturnsAsync(new CurrencyDataDto());

            // Act
            var result = await _controller.Get(correctCurrencyCode, date);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);


            // Act
            var result2 = await _controller.Get(incorrectCurrencyCode, date);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result2);
        }

        [Test]
        public async Task GetCurrencyData_WhenNullDatePassed()
        {
            // Arrange
            string currencyCode = "EUR";
            DateTime? date = null;
            DateTime testDate = DateTime.Today;

            _mockValidationSevice
                .Setup(v => v.IsValidCurrencyCode(currencyCode))
                .Returns(true);

            _mockValidationSevice.Setup(v => v.ValidateDate(testDate)).Returns(true);
            _mockValidationSevice.Setup(v => v.IsHoliday(testDate)).Returns(false);

            _mockCurrencyDataService.Setup(c => c.GetCurrencyDataFor(currencyCode, testDate)).ReturnsAsync(new CurrencyDataDto());

            // Act
            var result = await _controller.Get(currencyCode, date);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

    }
}