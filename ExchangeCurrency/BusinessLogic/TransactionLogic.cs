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

        public TransactionDto GetTransactionById(int id)
        {
            var user = _dbContext.Transactions.Find(id);
            var transactionView = _mapper.Map<TransactionDto>(user);
            return transactionView;
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

        public string Create(TransactionDto _transactionInformation)
        {
            var transactionInformation = new Transaction();
            try
            {
                var currencyExchangeRates = _dbContext.CurrencyExchangeRates.ToList();
                var currencyCode = _dbContext.Currency_Symbol.Single(c => c.Code == _transactionInformation.CurrencyType);
                var type = _transactionInformation.CurrencyType.ToString();
                var cuurrencyValue = Convert.ToUInt32(GetPropValue(currencyExchangeRates.Last(), _transactionInformation.CurrencyType));

                transactionInformation.QuantityReceived = _transactionInformation.AmountPurchased / cuurrencyValue;
                transactionInformation.CurrencyExchangeRate = currencyExchangeRates.Last();
                transactionInformation.DateExecution = DateTime.UtcNow;
                transactionInformation.TransactionStatus = "successful";
                transactionInformation.AmountPurchased = _transactionInformation.AmountPurchased;
                transactionInformation.CurrencyCode = currencyCode.Id;
                transactionInformation.CurrencyExchangeRateId = currencyExchangeRates.Last().Id;
                transactionInformation.CurrencyType = currencyCode.Code;
                _dbContext.Transactions.Add(transactionInformation);
                _dbContext.SaveChanges();
                return transactionInformation.TransactionStatus;
            }
            catch (Exception)
            {
                transactionInformation.TransactionStatus = "failed";
                _dbContext.SaveChanges();
                return transactionInformation.TransactionStatus;
            }
        }

        public string Update(Transaction transaction, int id)
        {
            try
            {
                _dbContext.Transactions.Attach(transaction);
                _dbContext.Entry(transaction).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return "successful";
            }
            catch (Exception)
            {
                return "failed";
            }
        }

        public string Delete(int id)
        {
            try
            {
                var transactionToDelete = _dbContext.Transactions.Single(m => m.Id == id);
                _dbContext.Transactions.Remove(transactionToDelete);
                _dbContext.SaveChanges();
                return "successful";
            }
            catch (Exception)
            {
                return "failed";
            }
        }

        public async Task<CurrencyExchangeRateDto> GetLatestExchangeRate()
        {
            var latest = await _dbContext.CurrencyExchangeRates.OrderBy(m => m.Id).LastAsync();
            return _mapper.Map<CurrencyExchangeRateDto>(latest);
        }

        public string AddLatestExchangeRate(CurrencyExchangeRateDto rateDto)
        {
            try
            {
                var rate = _mapper.Map<CurrencyExchangeRate>(rateDto);
                _dbContext.CurrencyExchangeRates.Add(rate);
                _dbContext.SaveChangesAsync();
                return "successful";
            }
            catch (Exception)
            {
                return "failed";
            }
        }
    }
}
