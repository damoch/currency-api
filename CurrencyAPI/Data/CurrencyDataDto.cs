namespace CurrencyAPI.Data
{
    public class CurrencyDataDto
    {
        public DateTime Date { get; set; }

        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SellRate { get; set; }

    }
}
