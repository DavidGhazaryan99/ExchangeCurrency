using ExchangeCurrency.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCurrency.DtoModels
{
    public class TransactionDto
    {
        public int id { get; set; }
        public DateTime DateExecution { get; set; }
        public string CurrencyType { get; set; }
        public decimal AmountPurchased { get; set; }
        public decimal QuantityReceived { get; set; }
    }
}
