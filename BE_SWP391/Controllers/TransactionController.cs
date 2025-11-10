using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] PaymentRequest request)
        {
            _logger.LogInformation("Received create-payment request for UserId: {UserId}, Amount: {Amount}", request.UserId, request.Amount);
            try
            {
                var result = _transactionService.CreatePaymentTransaction(request);
                if (result.Success)
                {
                    _logger.LogInformation("Payment created successfully for UserId: {UserId}", request.UserId);
                    return Ok(result);
                }
                else
                {
                    _logger.LogWarning("Payment creation failed for UserId: {UserId}: {Message}", request.UserId, result.Message);
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreatePayment for UserId: {UserId}", request.UserId);
                return BadRequest(new { message = ex.Message });  // Trả message chi tiết để debug
            }
        }

        [HttpGet("callback/vnpay")]
        public IActionResult VnPayCallback()
        {
            var queryString = HttpContext.Request.QueryString.ToString();  // Lấy query string đầy đủ
            _logger.LogInformation("Received VNPay callback: {QueryString}", queryString);
            try
            {
                bool isValid = _transactionService.HandleCallbackVnPay(queryString);
                if (isValid)
                {
                    _logger.LogInformation("VNPay callback processed successfully");
                    // Có thể redirect về trang thành công của frontend
                    return Ok(new { message = "Payment successful" });
                }
                else
                {
                    _logger.LogWarning("VNPay callback failed: Invalid signature or processing error");
                    return BadRequest(new { message = "Payment failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VnPayCallback");
                return BadRequest(new { message = ex.Message });
            }
        }

        // ===================== MoMo ======================
        [HttpPost("callback/momo")]
        public IActionResult MomoCallback([FromBody] MomoCallbackRequest callback)
        {
            Console.Clear();
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] 📩 MoMo Callback Received");

            var success = _transactionService.HandleCallbackMomo(callback);
            return Ok(new { success });
        }

        // ===================== Other APIs ======================
        [HttpGet("status/{transactionId}")]
        public IActionResult GetTransactionStatus(int transactionId)
        {
            _logger.LogInformation("Checking status for TransactionId: {TransactionId}", transactionId);
            try
            {
                var result = _transactionService.CheckTransactionStatus(transactionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking status for TransactionId: {TransactionId}", transactionId);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetRecent")]
        public IActionResult GetRecentTransaction(int count = 5)
        {
            return Ok(_transactionService.GetRecentTransactions(count));
        }

        [HttpGet("ReportTransaction")]
        public IActionResult GetReportTransaction()
        {
            return Ok(_transactionService.GetReportTransaction());
        }

        [HttpGet("RevenueStaff/Now/{userId}")]
        public IActionResult GetTransactionNow(int userId)
        {
            return Ok(_transactionService.GetTransactionNow(userId));
        }

        [HttpGet("RevenueStaff/TopBuyer/{userId}")]
        public IActionResult GetTopBuyers(int userId)
        {
            return Ok(_transactionService.GetTopBuyer(userId));
        }

        [HttpGet("RevenueStaff/DataRevenue/{userId}")]
        public IActionResult GetDataRevenueByUser(int userId)
        {
            return Ok(_transactionService.GetDataRevenueByUser(userId));
        }
        [HttpGet("debug-vnpay-signature")]
        public IActionResult DebugVnPaySignature()
        {
            var vnp_Params = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                ["vnp_Version"] = "2.1.0",
                ["vnp_Command"] = "pay",
                ["vnp_TmnCode"] = "TGLKAK6N",
                ["vnp_Amount"] = "1740000000",
                ["vnp_CreateDate"] = "20251110232941",
                ["vnp_CurrCode"] = "VND",
                ["vnp_ExpireDate"] = "20251110234441",
                ["vnp_IpAddr"] = "127.0.0.1",
                ["vnp_Locale"] = "vn",
                ["vnp_OrderInfo"] = "Invoice60",
                ["vnp_OrderType"] = "other",
                ["vnp_ReturnUrl"] = "https://bivalvular-untactfully-lili.ngrok-free.dev/api/Transaction/callback/vnpay",
                ["vnp_TxnRef"] = "60",
                ["vnp_Version"] = "2.1.0"
            };

            var rawHash = string.Join("&", vnp_Params.Select(kv => $"{kv.Key}={kv.Value}"));
            var signature = HmacSHA512("YZHS0UI8HK7MT2J945YG0QO81U28MX1Z", rawHash);

            return Ok(new
            {
                Parameters = vnp_Params,
                RawHash = rawHash,
                Signature = signature,
                TestWithVNPayTool = $"https://sandbox.vnpayment.vn/apis/docs/kiem-tra-tich-hop/?vnp_TmnCode=TGLKAK6N&vnp_HashSecret=YZHS0UI8HK7MT2J945YG0QO81U28MX1Z&rawHash={Uri.EscapeDataString(rawHash)}&signature={signature}"
            });
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
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

    }
}
