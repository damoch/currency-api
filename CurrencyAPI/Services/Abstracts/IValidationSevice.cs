namespace CurrencyAPI.Services.Abstracts
{
    public interface IValidationSevice
    {
        public bool ValidateDate(DateTime date);
        public Task<bool> IsHoliday(DateTime date);
        public bool IsValidCurrencyCode(string code);
    }
}
