using AutoMapper;
using ExchangeCurrency.Data.Models;
using ExchangeCurrency.DtoModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeCurrency.BusinessLogic
{
    public class TransactionLogic
    {
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TransactionLogic(AppDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<TransactionDto> GetTransactionById(int id)
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);
            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            return transactionDto;
        }

        public async Task<List<TransactionDto>> GetTransactionList()
        {
            var listTransaction = await _dbContext.Transactions.ToListAsync();
            List<TransactionDto> listTransactionDto = new List<TransactionDto>();
            for (int i = 0; i < listTransaction.Count; i++)
            {
                listTransactionDto.Add(_mapper.Map<TransactionDto>(listTransaction[i]));

            }
            return listTransactionDto;
        }

        public async Task Create(TransactionDto _transactionInformation)
        {
            var transactionInformation = new Transaction();
            try
            {
                var currencyExchangeRates = await _dbContext.CurrencyExchangeRates.ToListAsync();
                var currencyCode = _dbContext.Currency_Symbol.Single(c => c.Code == _transactionInformation.CurrencyType);
                var type = _transactionInformation.CurrencyType.ToString();
                var cuurrencyValue = Convert.ToUInt32(GetPropValue(currencyExchangeRates.Last(), _transactionInformation.CurrencyType));

                transactionInformation.QuantityReceived = _transactionInformation.AmountPurchased * cuurrencyValue;
                transactionInformation.CurrencyExchangeRate = currencyExchangeRates.Last();
                transactionInformation.DateExecution = DateTime.UtcNow;
                transactionInformation.TransactionStatus = "successful";
                transactionInformation.AmountPurchased = _transactionInformation.AmountPurchased;
                transactionInformation.CurrencyCode = currencyCode.Id;
                transactionInformation.CurrencyExchangeRateId = currencyExchangeRates.Last().Id;
                transactionInformation.CurrencyType = currencyCode.Code;
                _dbContext.Transactions.Add(transactionInformation);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                transactionInformation.TransactionStatus = "failed";
                _dbContext.SaveChanges();
                throw;
            }
        }

        public async Task Update(TransactionDto transaction)
        {
            var transactionInformation = new Transaction();
            try
            {
                var currencyExchangeRates = await _dbContext.CurrencyExchangeRates.ToListAsync();
                var currencyCode = _dbContext.Currency_Symbol.Single(c => c.Code == transaction.CurrencyType);
                var type = transaction.CurrencyType.ToString();
                var cuurrencyValue = Convert.ToUInt32(GetPropValue(currencyExchangeRates.Last(), transaction.CurrencyType));

                transactionInformation.QuantityReceived = transaction.AmountPurchased * cuurrencyValue;
                transactionInformation.CurrencyExchangeRate = currencyExchangeRates.Last();
                transactionInformation.DateExecution = DateTime.UtcNow;
                transactionInformation.TransactionStatus = "successful";
                transactionInformation.AmountPurchased = transaction.AmountPurchased;
                transactionInformation.CurrencyCode = currencyCode.Id;
                transactionInformation.CurrencyExchangeRateId = currencyExchangeRates.Last().Id;
                transactionInformation.CurrencyType = currencyCode.Code;
                _dbContext.Transactions.Attach(transactionInformation);
                _dbContext.Entry(transaction).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                transactionInformation.TransactionStatus = "failed";
                _dbContext.SaveChanges();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            var transactionToDelete = await _dbContext.Transactions.SingleAsync(m => m.Id == id);
            _dbContext.Transactions.Remove(transactionToDelete);
            _dbContext.SaveChanges();
        }

        public async Task<CurrencyExchangeRateDto> GetLatestExchangeRate()
        {
            var latest = await _dbContext.CurrencyExchangeRates.OrderBy(m => m.Id).LastAsync();
            return _mapper.Map<CurrencyExchangeRateDto>(latest);
        }

        public async Task AddLatestExchangeRate(CurrencyExchangeRateDto rateDto)
        {
            var rate = _mapper.Map<CurrencyExchangeRate>(rateDto);
            _dbContext.CurrencyExchangeRates.Add(rate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
