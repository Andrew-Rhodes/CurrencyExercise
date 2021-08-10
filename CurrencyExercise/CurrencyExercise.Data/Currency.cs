using System;

namespace CurrencyExercise.Data
{
    public class Currency
    {        
        public long CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime LastUpdatedTime { get; set; }
    }
}
