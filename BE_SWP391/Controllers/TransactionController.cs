using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Services.Implementations;
using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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

        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] PaymentRequest request)
        {
            try
            {
                var response = _transactionService.CreatePaymentTransaction(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // VNPay callback
        [HttpGet("callback/vnpay")]
        public IActionResult VnPayCallback()
        {
            var query = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
            var success = _transactionService.HandleCallbackVnPay(query);

            if (success)
                return Redirect("/payment-success");
            else
                return Redirect("/payment-failed");
        }

        // MoMo callback
        [HttpPost("callback/momo")]
        public IActionResult MomoCallback([FromBody] MomoCallbackRequest callback)
        {
            var success = _transactionService.HandleCallbackMomo(callback);
            return Ok(new { success });
        }

        // Kiểm tra trạng thái
        [HttpGet("status/{transactionId}")]
        public IActionResult CheckStatus(int transactionId)
        {
            var status = _transactionService.CheckTransactionStatus(transactionId);
            return Ok(status);
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
        [HttpGet("RevenueStaff/Now/{userId}")]
        public IActionResult GetTransactionNow(int userId)
        {
            var transactions = _transactionService.GetTransactionNow(userId);
            return Ok(transactions);
        }
        [HttpGet("RevenueStaff/TopBuyer/{userId}")]
        public IActionResult GetTopBuyers(int userId)
        {
            var topBuyers = _transactionService.GetTopBuyer(userId);
            return Ok(topBuyers);
        }
        [HttpGet("RevenueStaff/DataRevenue/{userId}")]
        public IActionResult GetDataRevenueByUser(int userId)
        {
            var dataRevenues = _transactionService.GetDataRevenueByUser(userId);
            return Ok(dataRevenues);
        }

        [HttpGet("test-simple-hmac")]
        public IActionResult TestSimpleHmac()
        {
            try
            {
                var key = "YZHS0UI8HK7MT2J945YG0QO81U28MX1Z";
                var message = "abc"; // Message cực kỳ đơn giản

                var signature = HmacSHA512(key, message);

                return Ok(new
                {
                    Message = message,
                    Signature = signature,
                    SignatureLength = signature.Length,
                    Note = "Signature should be 128 characters for SHA512"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2")); // ✅ Dùng "x2"
                }
            }

            return hash.ToString();
        }



    }
}
