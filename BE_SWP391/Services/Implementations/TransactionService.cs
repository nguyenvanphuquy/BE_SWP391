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

        public TransactionService(ITransactionRepository transactionRepository, IConfiguration config, EvMarketContext context)
        {
            _transactionRepository = transactionRepository;
            _config = config;
            _context = context;

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


        public string CreateVnPayUrl(List<Cart> carts, Invoice invoice, PaymentRequest req)
        {

            var totalAmount = (long)(invoice.TotalAmount * 100);
            var vnp_TxnRef = invoice.InvoiceId.ToString();
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_ExpireDate = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");
            var orderInfo = $"Thanh toan hoa don #{vnp_TxnRef}";
            var vnp_Params = new SortedDictionary<string, string>(StringComparer.Ordinal)
    {
        { "vnp_Version", "2.1.0" },
        { "vnp_Command", "pay" },
        { "vnp_TmnCode", vnp_TmnCode },
        { "vnp_Amount", totalAmount.ToString() },
        { "vnp_CreateDate", vnp_CreateDate },
        { "vnp_CurrCode", "VND" },
        { "vnp_IpAddr", "127.0.0.1" },
        { "vnp_Locale", "vn" },
        { "vnp_OrderInfo", orderInfo },
        { "vnp_OrderType", "other" },
        { "vnp_ReturnUrl", vnp_Returnurl },
        { "vnp_TxnRef", vnp_TxnRef },
        { "vnp_ExpireDate", vnp_ExpireDate }
    };

            var rawData = string.Join("&", vnp_Params.Select(kv => $"{kv.Key}={kv.Value}"));

            var vnp_SecureHash = HmacSHA512(vnp_HashSecret, rawData);
            var queryString = string.Join("&", vnp_Params.Select(kv =>
                $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

            var paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={vnp_SecureHash}";

            // 5️⃣ Log ra console để debug khi cần
            Console.WriteLine("===== VNPay Debug =====");
            Console.WriteLine("RawData: " + rawData);
            Console.WriteLine("SecureHash: " + vnp_SecureHash);
            Console.WriteLine("Final URL: " + paymentUrl);
            Console.WriteLine("=======================");

            return paymentUrl;
        }
        public bool HandleCallbackVnPay(IDictionary<string, string> query)
        {
            try
            {
                if (query == null || !query.Any())
                {
                    Console.WriteLine("⚠️ Callback rỗng hoặc thiếu dữ liệu");
                    return false;
                }

                if (!query.ContainsKey("vnp_SecureHash"))
                {
                    Console.WriteLine("⚠️ Thiếu vnp_SecureHash");
                    return false;
                }

                var secureHash = query["vnp_SecureHash"];

                // ✅ Chỉ lấy các key bắt đầu bằng vnp_ (trừ hash)
                var keys = query.Keys
                    .Where(k => k.StartsWith("vnp_") && k != "vnp_SecureHash" && k != "vnp_SecureHashType")
                    .OrderBy(k => k, StringComparer.Ordinal)
                    .ToList();

                // ✅ KHÔNG UrlDecode vì ASP.NET đã decode sẵn
                var rawData = string.Join("&", keys.Select(k => $"{k}={query[k]}"));

                // ✅ Tính lại hash
                var computedHash = HmacSHA512(vnp_HashSecret, rawData);

                // ✅ Log để debug
                Console.WriteLine("===== VNPay Callback Debug =====");
                Console.WriteLine("RawData: " + rawData);
                Console.WriteLine("Computed: " + computedHash);
                Console.WriteLine("Received: " + secureHash);
                Console.WriteLine("===============================");

                // ✅ So sánh chữ ký
                if (!string.Equals(computedHash, secureHash, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("❌ Sai chữ ký VNPay callback");
                    return false;
                }

                // ✅ Nếu chữ ký đúng → xử lý tiếp
                if (!query.ContainsKey("vnp_TxnRef"))
                    return false;

                int invoiceId = int.Parse(query["vnp_TxnRef"]);
                var responseCode = query.ContainsKey("vnp_ResponseCode") ? query["vnp_ResponseCode"] : "99";
                var transactionStatus = query.ContainsKey("vnp_TransactionStatus") ? query["vnp_TransactionStatus"] : "99";

                var transactions = _context.Transactions
                    .Include(t => t.Invoice)
                    .Where(t => t.InvoiceId == invoiceId)
                    .ToList();

                if (!transactions.Any())
                {
                    Console.WriteLine($"⚠️ Không tìm thấy transactions cho Invoice #{invoiceId}");
                    return false;
                }

                var invoice = transactions.First().Invoice;
                if (invoice == null)
                    return false;

                if (responseCode == "00" && transactionStatus == "00")
                {
                    foreach (var t in transactions)
                    {
                        t.Status = "completed";
                        t.TransactionDate = DateTime.UtcNow;
                    }

                    invoice.DueDate = DateOnly.FromDateTime(DateTime.UtcNow);

                    var planIdsLinkedToInvoice = transactions.Select(t => t.PlanId).ToList();
                    var cartsToUpdate = _context.Carts
                        .Where(c => c.UserId == invoice.UserId
                                 && c.Status == "pending"
                                 && planIdsLinkedToInvoice.Contains(c.PlanId))
                        .ToList();

                    foreach (var cart in cartsToUpdate)
                    {
                        cart.Status = "paid";
                        cart.UpdatedAt = DateTime.UtcNow;
                    }

                    _context.SaveChanges();
                    Console.WriteLine($"✅ VNPay thanh toán thành công cho Invoice #{invoiceId}");
                    return true;
                }
                else
                {
                    foreach (var transaction in transactions)
                    {
                        transaction.Status = "failed";
                    }

                    _context.SaveChanges();
                    Console.WriteLine($"❌ VNPay thanh toán thất bại: ResponseCode={responseCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi xử lý VNPay callback: {ex.Message}");
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