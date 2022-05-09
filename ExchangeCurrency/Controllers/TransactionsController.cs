using ExchangeCurrency.BusinessLogic;
using ExchangeCurrency.Controllers.Attributes;
using ExchangeCurrency.Data.Models;
using ExchangeCurrency.DtoModels;
using ExchangeCurrency.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeCurrency.Controllers
{
    [ApiKeyAuthorization]
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionLogic _transactionLogic;
        private ILog logger;

        public TransactionsController(TransactionLogic transactionLogic, ILog logger)
        {
            _transactionLogic = transactionLogic;
            this.logger = logger;
        }

        [HttpGet("list")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var transactionList = await _transactionLogic.GetTransactionList();
                if (transactionList != null)
                    return Ok(transactionList);
                else
                    return NotFound("Requested resource not found");
            }
            catch (System.Exception ex)
            {
                logger.Error("transaction list get failed", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransaction(int id)
        {
            try
            {
                var transaction = await _transactionLogic.GetTransactionById(id);
                if (transaction != null)
                    return Ok(transaction);
                else
                    return NotFound("Requested resource not found");
            }
            catch (System.Exception ex)
            {
                logger.Error("transaction get failed", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Post([FromBody] TransactionDto transactionInformation)
        {
            try
            {
                await _transactionLogic.Create(transactionInformation);
                logger.Information("Trasaction Created");
                return Ok("transaction created");
            }
            catch (System.Exception ex)
            {
                logger.Error("transaction failed", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(TransactionDto transaction)
        {
            try
            {
                await _transactionLogic.Update(transaction);
                logger.Information("Trasaction Updated");
                return Ok("transaction updated");
            }
            catch (System.Exception ex)
            {
                logger.Error($"Could not update transaction with id {transaction.id} not updated", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _transactionLogic.Delete(id);
                logger.Information("Trasaction deleted");
                return Ok($"Transaction with id {id} deleted");
            }
            catch (System.Exception ex)
            {
                logger.Error($"Could not delete transaction with id {id}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
