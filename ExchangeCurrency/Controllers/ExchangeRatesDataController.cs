using ExchangeCurrency.BusinessLogic;
using ExchangeCurrency.Controllers.Attributes;
using ExchangeCurrency.DtoModels;
using ExchangeCurrency.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeCurrency.Controllers
{
    [ApiKeyAuthorization]
    [Route("exchangerates_data")]
    [ApiController]
    public class ExchangeRatesDataController : ControllerBase
    {
        private readonly TransactionLogic _transactionLogic;
        private ILog logger;

        public ExchangeRatesDataController(TransactionLogic transactionLogic, ILog logger)
        {
            _transactionLogic = transactionLogic;
            this.logger = logger;
        }

        [HttpGet("latest")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var latestRate = await _transactionLogic.GetLatestExchangeRate();
                if (latestRate == null)
                    return NotFound("rate does not exists");
                else
                    return Ok(latestRate);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostExchangeRate(CurrencyExchangeRateDto rateDto)
        {
            try
            {
                await _transactionLogic.AddLatestExchangeRate(rateDto);
                return Ok("transaction updated");
            }
            catch (System.Exception ex)
            {
                logger.Error("transaction not Deleted", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
