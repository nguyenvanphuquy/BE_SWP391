using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using BE_SWP391.Data;
using System.Security.Cryptography;

namespace BE_SWP391.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly EvMarketContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _http = new HttpClient();
        private readonly ILogger<TransactionService> _logger;
        // config values
        private readonly string vnp_TmnCode;
        private readonly string vnp_HashSecret;
        private readonly string vnp_Url;
        private readonly string vnp_Returnurl;

        private readonly string momo_PartnerCode;
        private readonly string momo_AccessKey;
        private readonly string momo_SecretKey;
        private readonly string momo_Endpoint;
        private readonly string momo_IpnUrl;
        private readonly string momo_RedirectUrl;

        public TransactionService(ITransactionRepository transactionRepository, IConfiguration config, EvMarketContext context, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _config = config;
            _context = context;
            _logger = logger;

            var vn = _config.GetSection("Payment:VnPay");
            vnp_TmnCode = vn["TmnCode"];
            vnp_HashSecret = vn["HashSecret"];
            vnp_Url = vn["Url"];
            vnp_Returnurl = vn["ReturnUrl"];

            var mm = _config.GetSection("Payment:MoMo");
            momo_PartnerCode = mm["PartnerCode"];
            momo_AccessKey = mm["AccessKey"];
            momo_SecretKey = mm["SecretKey"];
            momo_Endpoint = mm["Endpoint"];
            momo_IpnUrl = mm["IpnUrl"];
            momo_RedirectUrl = mm["RedirectUrl"];
        }

        public PaymentCreateResponse CreatePaymentTransaction(PaymentRequest request)
        {
            using var dbTransaction = _context.Database.BeginTransaction();

            try
            {
                // 1️⃣ Validate giỏ hàng
                var carts = _context.Carts
                    .Include(c => c.Plan)
                    .Where(c => request.CartIds.Contains(c.CartId) && c.UserId == request.UserId)
                    .ToList();

                if (carts == null || !carts.Any())
                    throw new Exception("Không tìm thấy giỏ hàng hợp lệ.");
                var totalAmount = carts.Sum(c => (c.Plan?.Price ?? 0) * c.Quantity);
                // 2️⃣ Tạo Invoice
                var invoice = new Invoice
                {
                    InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{request.UserId}",
                    UserId = request.UserId,
                    TotalAmount = request.Amount,
                    IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Currency = "VND"
                };
                _context.Invoices.Add(invoice);
                _context.SaveChanges();
                foreach (var cart in carts)
                {
                    var transaction = new Transaction
                    {
                        TransactionDate = DateTime.UtcNow,
                        Amount = cart.Plan.Price * cart.Quantity,
                        Currency = "VND",
                        Status = "pending",
                        InvoiceId = invoice.InvoiceId,
                        PlanId = cart.PlanId  // ✅ thêm dòng này để không lỗi FOREIGN KEY
                    };
                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();
                    // 3️⃣ Tạo Transaction với status = "pending"
                    var paymentMethod = new PaymentMethod
                    {
                        MethodName = request.PaymentMethod.ToUpper(),
                        Provider = request.PaymentMethod.ToUpper(),
                        Status = "active",
                        TransactionId = transaction.TransactionId
                    };
                    _context.PaymentMethods.Add(paymentMethod);
                    _context.SaveChanges();
                }

                _context.SaveChanges();
                dbTransaction.Commit();



                // 5️⃣ Tạo payment URL dựa vào phương thức
                if (request.PaymentMethod.ToLower() == "vnpay")
                {
                    var paymentUrl = CreateVnPayUrl(carts, invoice, request);
                    return new PaymentCreateResponse
                    {
                        Success = true,
                        PaymentUrl = paymentUrl,
                        TransactionId = 0,
                        OrderId = invoice.InvoiceId.ToString(),
                        Amount = request.Amount,
                        PaymentMethod = "VNPay",
                        Message = "Vui lòng thanh toán qua VNPay"
                    };
                }
                else if (request.PaymentMethod.ToLower() == "momo")
                {
                    var momoResponse = CreateMomoPaymentUrl(carts, invoice, request);
                    return new PaymentCreateResponse
                    {
                        Success = true,
                        PaymentUrl = momoResponse.PayUrl,
                        QrCodeUrl = momoResponse.QrCodeUrl,
                        TransactionId = 0,
                        OrderId = invoice.InvoiceId.ToString(),
                        Amount = request.Amount,
                        PaymentMethod = "MoMo",
                        Message = "Vui lòng quét mã QR để thanh toán"
                    };
                }
                else
                {
                    throw new Exception("Phương thức thanh toán không được hỗ trợ.");
                }
            }

            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("❌ Lỗi khi lưu dữ liệu vào DB: " + inner);
            }
            catch (Exception ex)
            {
                throw new Exception("❌ Lỗi chung: " + ex.Message);
            }

        }


        private string CreateVnPayUrl(List<Cart> carts, Invoice invoice, PaymentRequest req)
        {
            var totalAmount = (long)(invoice.TotalAmount * 100);
            var vnp_OrderId = invoice.InvoiceId.ToString();
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_ExpireDate = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");
            var orderInfo = "Invoice" + vnp_OrderId;

            // ✅ ĐÚNG: Encode URL từ appsettings hoặc hardcode
            var returnUrl = Uri.EscapeDataString(vnp_Returnurl);
            // Hoặc: var returnUrl = Uri.EscapeDataString("https://bivalvular-untactfully-lili.ngrok-free.dev/api/Transaction/callback/vnpay");

            var vnp_Params = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                ["vnp_Version"] = "2.1.0",
                ["vnp_Command"] = "pay",
                ["vnp_TmnCode"] = vnp_TmnCode,
                ["vnp_Amount"] = totalAmount.ToString(),
                ["vnp_CreateDate"] = vnp_CreateDate,
                ["vnp_CurrCode"] = "VND",
                ["vnp_IpAddr"] = "127.0.0.1",
                ["vnp_Locale"] = "vn",
                ["vnp_OrderInfo"] = orderInfo,
                ["vnp_OrderType"] = "other",
                ["vnp_ReturnUrl"] = returnUrl, // ✅ Đã encode
                ["vnp_TxnRef"] = vnp_OrderId,
                ["vnp_ExpireDate"] = vnp_ExpireDate
            };

            // ✅ rawHash giờ có URL đã encode
            var rawHash = string.Join("&", vnp_Params.Select(kv => $"{kv.Key}={kv.Value}"));

            _logger.LogInformation("=== VNPAY DEBUG ===");
            _logger.LogInformation("Raw Hash: {RawHash}", rawHash);

            var signature = HmacSHA512(vnp_HashSecret, rawHash);

            _logger.LogInformation("Signature: {Signature}", signature);
            _logger.LogInformation("=== END DEBUG ===");

            // ✅ ĐÚNG: Không encode lần 2 vì đã encode rồi
            var queryString = string.Join("&", vnp_Params.Select(kv => $"{kv.Key}={kv.Value}"));

            var paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={signature}";

            return paymentUrl;
        }

        public bool HandleCallbackVnPay(string queryString)
        {
            _logger.LogInformation("Handling VNPay callback");

            if (string.IsNullOrEmpty(queryString))
            {
                _logger.LogWarning("Query string is empty.");
                return false;
            }

            // ✅ Loại bỏ dấu '?' nếu có
            if (queryString.StartsWith("?"))
                queryString = queryString.Substring(1);

            // ✅ QUAN TRỌNG: Parse query string và giữ nguyên giá trị chưa decode
            var queryParams = new Dictionary<string, string>();

            foreach (var pair in queryString.Split('&'))
            {
                var parts = pair.Split('=');
                if (parts.Length == 2)
                {
                    // ✅ GIỮ NGUYÊN giá trị chưa decode để tính chữ ký
                    queryParams[parts[0]] = parts[1];
                }
            }

            if (!queryParams.ContainsKey("vnp_SecureHash"))
            {
                _logger.LogWarning("Missing vnp_SecureHash.");
                return false;
            }

            var receivedHash = queryParams["vnp_SecureHash"];

            // ✅ Lấy tất cả key vnp_ trừ vnp_SecureHash và vnp_SecureHashType
            var keys = queryParams.Keys
                .Where(k => k.StartsWith("vnp_") && k != "vnp_SecureHash" && k != "vnp_SecureHashType")
                .OrderBy(k => k, StringComparer.Ordinal)
                .ToList();

            // ✅ Tạo rawData với giá trị CHƯA DECODE (quan trọng!)
            var rawData = string.Join("&", keys.Select(k => $"{k}={queryParams[k]}"));

            var computedHash = HmacSHA512(vnp_HashSecret, rawData);

            _logger.LogDebug("VNPay Callback - RawData: {RawData}", rawData);
            _logger.LogDebug("Computed Hash: {Computed}", computedHash);
            _logger.LogDebug("Received Hash: {Received}", receivedHash);

            // ✅ So sánh chữ ký
            if (!computedHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("VNPay signature mismatch.");
                return false;
            }

            // ✅ Chữ ký hợp lệ, xử lý giao dịch
            var txnRef = HttpUtility.UrlDecode(queryParams["vnp_TxnRef"]);
            var responseCode = HttpUtility.UrlDecode(queryParams["vnp_ResponseCode"]);

            using var dbTransaction = _context.Database.BeginTransaction();
            try
            {
                // ✅ TxnRef là InvoiceId, không phải TransactionId
                var invoiceId = int.Parse(txnRef);

                var transactions = _context.Transactions
                    .Include(t => t.Invoice)
                    .Where(t => t.InvoiceId == invoiceId)
                    .ToList();

                if (transactions == null || !transactions.Any())
                {
                    _logger.LogWarning("Transaction not found for InvoiceId: {InvoiceId}", invoiceId);
                    return false;
                }

                if (responseCode == "00")  // ✅ Thành công
                {
                    foreach (var transaction in transactions)
                    {
                        transaction.Status = "completed";
                        transaction.TransactionDate = DateTime.UtcNow;
                    }

                    // ✅ Cập nhật Invoice
                    var invoice = transactions.First().Invoice;
                    if (invoice != null)
                    {
                        invoice.Status = "paid";
                        invoice.DueDate = DateOnly.FromDateTime(DateTime.UtcNow);

                        // ✅ Cập nhật cart status
                        var carts = _context.Carts
                            .Where(c => c.UserId == invoice.UserId && c.Status == "pending")
                            .ToList();

                        foreach (var cart in carts)
                        {
                            cart.Status = "paid";
                            cart.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                }
                else
                {
                    // ✅ Thất bại
                    foreach (var transaction in transactions)
                    {
                        transaction.Status = "failed";
                    }
                }

                _context.SaveChanges();
                dbTransaction.Commit();

                _logger.LogInformation("VNPay callback processed successfully for InvoiceId: {InvoiceId}, Status: {Status}",
                    invoiceId, responseCode == "00" ? "Success" : "Failed");

                return true;
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                _logger.LogError(ex, "Error processing VNPay callback");
                return false;
            }
        }
        private MomoPaymentResponse CreateMomoPaymentUrl(List<Cart> carts, Invoice invoice, PaymentRequest req)
        {
            try
            {
                var orderId = invoice.InvoiceId.ToString();
                var requestId = Guid.NewGuid().ToString();
                var amount = ((long)invoice.TotalAmount).ToString();
                var packageNames = string.Join(", ", carts.Select(c => c.Plan?.PlanName ?? "Gói dữ liệu"));
                var orderInfo = $"Thanh toán {carts.Count} gói dữ liệu: {packageNames}";
                var rawHash = $"accessKey={momo_AccessKey}" +
                             $"&amount={amount}" +
                             $"&extraData=" +
                             $"&ipnUrl={momo_IpnUrl}" +
                             $"&orderId={orderId}" +
                             $"&orderInfo={orderInfo}" +
                             $"&partnerCode={momo_PartnerCode}" +
                             $"&redirectUrl={momo_RedirectUrl}" +
                             $"&requestId={requestId}" +
                             $"&requestType=captureWallet";

                var signature = HmacSHA256(momo_SecretKey, rawHash);

                // 🧰 Request body gửi đến MoMo
                var requestBody = new
                {
                    partnerCode = momo_PartnerCode,
                    partnerName = "EV Data Marketplace",
                    storeId = momo_PartnerCode,
                    requestId = requestId,
                    amount = amount,
                    orderId = orderId,
                    orderInfo = orderInfo,
                    redirectUrl = momo_RedirectUrl,
                    ipnUrl = momo_IpnUrl,
                    lang = "vi",
                    extraData = "",
                    requestType = "captureWallet",
                    signature = signature
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🚀 Gửi yêu cầu đến MoMo
                var response = _http.PostAsync(momo_Endpoint, content).Result;
                var responseStr = response.Content.ReadAsStringAsync().Result;

                var momoResponse = JsonConvert.DeserializeObject<MomoPaymentResponse>(responseStr);

                if (momoResponse == null)
                    throw new Exception("Không nhận được phản hồi hợp lệ từ MoMo.");

                if (momoResponse.ResultCode != 0)
                    throw new Exception($"MoMo Error ({momoResponse.ResultCode}): {momoResponse.Message}");

                return momoResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gọi API MoMo: {ex.Message}");
            }
        }
        public bool HandleCallbackMomo(MomoCallbackRequest callback)
        {
            try
            {
                var rawHash = $"accessKey={momo_AccessKey}" +
                             $"&amount={callback.Amount}" +
                             $"&extraData={callback.ExtraData}" +
                             $"&message={callback.Message}" +
                             $"&orderId={callback.OrderId}" +
                             $"&orderInfo={callback.OrderInfo}" +
                             $"&orderType={callback.OrderType}" +
                             $"&partnerCode={momo_PartnerCode}" +
                             $"&payType={callback.PayType}" +
                             $"&requestId={callback.RequestId}" +
                             $"&responseTime={callback.ResponseTime}" +
                             $"&resultCode={callback.ResultCode}" +
                             $"&transId={callback.TransId}";

                var signature = HmacSHA256(momo_SecretKey, rawHash);

                if (!string.Equals(signature, callback.Signature, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("⚠️ MoMo signature không hợp lệ");
                    return false;
                }
                var transactionId = int.Parse(callback.OrderId);
                var transaction = _context.Transactions
                    .Include(t => t.Invoice)
                    .FirstOrDefault(t => t.TransactionId == transactionId);

                if (transaction == null)
                {
                    Console.WriteLine($"⚠️ Không tìm thấy transaction #{transactionId}");
                    return false;
                }
                if (callback.ResultCode == 0)
                {
                    transaction.Status = "completed";
                    transaction.TransactionDate = DateTime.UtcNow;

                    if (transaction.Invoice != null)
                    {
                        transaction.Invoice.DueDate = DateOnly.FromDateTime(DateTime.UtcNow);
                    }
                    var invoice = transaction.Invoice;
                    if (invoice != null)
                    {
                        var carts = _context.Carts
                            .Where(c => c.UserId == invoice.UserId && c.Status == "pending")
                            .ToList();

                        foreach (var cart in carts)
                        {
                            cart.Status = "paid";
                            cart.UpdatedAt = DateTime.UtcNow;
                        }
                    }

                    _context.SaveChanges();

                    Console.WriteLine($"✅ MoMo thanh toán thành công cho transaction #{transactionId}");
                    return true;
                }
                else
                {
                    transaction.Status = "failed";
                    _context.SaveChanges();

                    Console.WriteLine($"❌ MoMo thanh toán thất bại: {callback.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi xử lý MoMo callback: {ex.Message}");
                return false;
            }
        }
        public TransactionStatusResponse CheckTransactionStatus(int transactionId)
        {
            var transaction = _context.Transactions
                .Include(t => t.Invoice)
                .Include(t => t.PaymentMethods)
                .FirstOrDefault(t => t.TransactionId == transactionId);

            if (transaction == null)
                throw new Exception("Không tìm thấy giao dịch.");

            return new TransactionStatusResponse
            {
                TransactionId = transaction.TransactionId,
                Status = transaction.Status,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                InvoiceNumber = transaction.Invoice?.InvoiceNumber,
                PaymentMethod = transaction.PaymentMethods?.FirstOrDefault()?.MethodName
            };
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
        private static string HmacSHA256(string key, string input)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }


        public List<RecentTransactionResponse> GetRecentTransactions(int count = 5)
        {
            return _transactionRepository.GetRecentTransaction();
        }
        public ReportTransactionResponse GetReportTransaction()
        {
            return _transactionRepository.GetReportTransaction();
        }
        public List<TransactionNowResponse> GetTransactionNow(int userId)
        {
            return _transactionRepository.GetTransactionNow(userId);
        }
        public List<TopBuyerResponse> GetTopBuyer(int userId)
        {
            return _transactionRepository.GetTopBuyers(userId);
        }
        public List<DataRevenueResponse> GetDataRevenueByUser(int userId)
        {
            return _transactionRepository.GetDataRevenueByUser(userId);
        }
    }
}