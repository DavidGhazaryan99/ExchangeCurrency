using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCurrency.Data.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateExecution { get; set; }
        public string CurrencyType { get; set; }
        public decimal CurrencyCode { get; set; }
        public CurrencyExchangeRate CurrencyExchangeRate { get; set; }
        public int CurrencyExchangeRateId { get; set; }
        public decimal AmountPurchased { get; set; }
        public decimal QuantityReceived { get; set; }
        public string TransactionStatus { get; set; }
    }
}
