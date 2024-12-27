﻿namespace CurrencyAPI.Data
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }

}
