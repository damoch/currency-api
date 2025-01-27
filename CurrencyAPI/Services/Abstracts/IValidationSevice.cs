namespace CurrencyAPI.Services.Abstracts
{
    public interface IValidationSevice
    {
        public bool ValidateDate(DateTime date);
        public bool IsHoliday(DateTime date);
        public bool IsValidCurrencyCode(string code);
        DateTime OldestPossibleDate { get; }
        string[] AvailableCurrencies { get; }
    }
}
