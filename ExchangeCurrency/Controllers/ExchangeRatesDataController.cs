using ExchangeCurrency.BusinessLogic;
using ExchangeCurrency.Controllers.Attributes;
using ExchangeCurrency.DtoModels;
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

        public ExchangeRatesDataController(TransactionLogic transactionLogic)
        {
            _transactionLogic = transactionLogic;
        }

        [HttpGet("latest")]
        public async Task<object> Get()
        {
            if (await _transactionLogic.GetLatestExchangeRate() != null)
                return await _transactionLogic.GetLatestExchangeRate();
            else
                return NotFound("Can Not Find Requested Resource");
        }
        [HttpPost]
        public object PostExchangeRate(CurrencyExchangeRateDto rateDto)
        {
            string status = _transactionLogic.AddLatestExchangeRate(rateDto);
            if (status == "successful")
                return Ok("transaction updated");
            else
                return NotFound(status);
        }
    }
}
