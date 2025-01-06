using Microsoft.EntityFrameworkCore;

namespace CurrencyAPI.Data
{
    public class CurrencyRate
    {
        public int Id { get; set; } // Technicznie niepotrzebny, ale zachowuje go dla celow ewentualnej dalszej rozbudowy
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public DateTime Date { get; set; }

        public CurrencyDataDto AsDto()
        {
            return new CurrencyDataDto
            {
                CurrencyCode = CurrencyCode,
                CurrencyName = CurrencyName,
                PurchaseRate = BuyRate,
                SellRate = SellRate,
                Date = Date
            };
        }
        public static CurrencyRate FromDto(CurrencyDataDto dto)
        {
            return new CurrencyRate() {
                CurrencyCode = dto.CurrencyCode,
                CurrencyName= dto.CurrencyName,
                SellRate = dto.SellRate,
                Date = dto.Date,
                BuyRate = dto.PurchaseRate
            };
        }

    }
}
