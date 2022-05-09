using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCurrency.Data.Models
{
    public class CurrencyExchangeRate
    {
        public int Id { get; set; }
        public decimal USD { get; set; }
        public decimal EUR { get; set; }
        public decimal RUR { get; set; }
    }
}
