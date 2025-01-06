using CurrencyAPI.Services.Abstracts;
using CurrencyAPI.Services.Implementations;

namespace CurrencyAPITests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        private IValidationSevice _validationService;

        [SetUp]
        public void Setup()
        {
            _validationService = new NBPApiValidationService();
        }

        [Test]
        public void ValidateDate_InvalidDateTest()
        {
            DateTime dateInFuture = DateTime.Now.AddDays(1);
            DateTime dateInThePast = new DateTime(1970, 1, 1);

            Assert.That(!_validationService.ValidateDate(dateInFuture));
            Assert.That(!_validationService.ValidateDate(dateInThePast));
        }

        [Test]
        public void ValidateDate_HolidayTest()
        { 
            DateTime saturday = new DateTime(2025, 1, 4);

            Assert.That(_validationService.IsHoliday(saturday));
        }

        [Test]
        public void ValidateCurrencyCode_NullAndEmptyCheck()
        {
            Assert.That(!_validationService.IsValidCurrencyCode(null));
            Assert.That(!_validationService.IsValidCurrencyCode(""));
        }

        [Test]
        public void ValidateCurrencyCode_CorrectCodeCheck()
        {
            Assert.That(_validationService.IsValidCurrencyCode("EUR"));
            Assert.That(_validationService.IsValidCurrencyCode("usd"));
        }
    }
}
