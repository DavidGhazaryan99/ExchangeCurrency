using AutoMapper;
using ExchangeCurrency.BusinessLogic;
using ExchangeCurrency.Controllers.Attributes;
using ExchangeCurrency.Data.Models;
using ExchangeCurrency.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeCurrency.Controllers
{
    [ApiKeyAuthorization]
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly TransactionLogic _transactionLogic;

        public TransactionsController(ILogger<TransactionsController> logger, TransactionLogic transactionLogic)
        {
            _transactionLogic = transactionLogic;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<object> Get()
        {
            if (await _transactionLogic.GetTransactionList() != null)
                return await _transactionLogic.GetTransactionList();
            else
                return NotFound("Can Not Find Requested Resource");
        }

        [HttpGet("{id}")]
        public object GetTransaction(int id)
        {
            if (_transactionLogic.GetTransactionById(id) != null)
                return _transactionLogic.GetTransactionById(id);
            else
                return NotFound("Can Not Find Requested Resource");
        }

        [HttpPost("Create")]
        public object Post([FromBody] TransactionDto transactionInformation)
        {
            string status = _transactionLogic.Create(transactionInformation);
            if (status == "successful")
                return Ok("transaction created");
            else
                return NotFound(status);

        }

        [HttpPut("{id}")]
        public object Put(Transaction transaction, int id)
        {
            string status = _transactionLogic.Update(transaction, id);
            if (status == "successful")
                return Ok("transaction updated");
            else
                return NotFound(status);
        }

        [HttpDelete("{id}")]
        public object Delete(int id)
        {
            var status = _transactionLogic.Delete(id);
            if (status == "successful")
                return Ok("transaction deleted");
            else
                return NotFound(status);
        }
    }
}
