using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] PaymentRequest request)
        {
            try
            {
                var res = _transactionService.CreatePayment(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // VNPay return (GET)
        [HttpGet("callback/vnpay")]
        public IActionResult VnPayCallback()
        {
            var query = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
            var ok = _transactionService.HandleCallbackVnPay(query);
            if (ok) return Ok("Payment success");
            return BadRequest("Payment verify failed");
        }

        // MoMo IPN (POST JSON)
        [HttpPost("callback/momo")]
        public IActionResult MomoCallback([FromBody] Dictionary<string, string> body)
        {
            var ok = _transactionService.HandleCallbackMomo(body);
            if (ok) return Ok(new { result = "success" });
            return BadRequest(new { result = "failed" });
        }
        [HttpGet("GetRecent")]
        public IActionResult GetRecentTransaction(int count = 5)
        {
            return Ok(_transactionService.GetRecentTransactions());
        }
        [HttpGet("ReportTransaction")]
        public IActionResult GetReportTransaction()
        {
            return Ok(_transactionService.GetReportTransaction());
        }
    }
}
