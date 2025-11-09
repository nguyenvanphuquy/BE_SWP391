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
        private readonly string momo_Returnurl;

        public TransactionService(ITransactionRepository transactionRepository, IConfiguration config)
        {
            _transactionRepository = transactionRepository;
            _config = config;
            var payCfg = _config.GetSection("Payment");
            var vn = payCfg.GetSection("VnPay");
            vnp_TmnCode = vn["TmnCode"];
            vnp_HashSecret = vn["HashSecret"];
            vnp_Url = vn["Url"];
            vnp_Returnurl = vn["ReturnUrl"];

            var mm = payCfg.GetSection("MoMo");
            momo_PartnerCode = mm["PartnerCode"];
            momo_AccessKey = mm["AccessKey"];
            momo_SecretKey = mm["SecretKey"];
            momo_Endpoint = mm["Endpoint"];
            momo_Returnurl = mm["ReturnUrl"];
        }

        //public PaymentCreateResponse CreatePayment(PaymentRequest request)
        //{
        public string CreateTransaction(int userId, int[] cartIds, decimal totalAmount)
        {
            // 1️⃣ Tạo hóa đơn
            var invoice = new Invoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{userId}",
                UserId = userId,
                TotalAmount = totalAmount,
                IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Currency = "VND"
            };
            _context.Invoices.Add(invoice);

            // 2️⃣ Tạo giao dịch
            var transaction = new Transaction
            {
                TransactionDate = DateTime.UtcNow,
                Amount = totalAmount,
                Currency = "VND",
                Status = "success",  // vì không qua cổng thanh toán, coi như đã thanh toán thành công
                Invoice = invoice
            };
            _context.Transactions.Add(transaction);

            // 3️⃣ Lấy giỏ hàng và đổi trạng thái
            var carts = _context.Carts.Where(c => cartIds.Contains(c.CartId) && c.UserId == userId).ToList();

            if (carts == null || !carts.Any())
                throw new Exception("Không tìm thấy giỏ hàng hợp lệ cho người dùng này.");

            foreach (var cart in carts)
            {
                cart.Status = "paid"; // hoặc “checkedout”
            }

            // 4️⃣ Lưu thay đổi
            _context.SaveChanges();

            return $"Giao dịch {transaction.TransactionId} đã được tạo thành công và {carts.Count} giỏ hàng đã cập nhật trạng thái.";
        
        //    // 1) tạo Invoice + Transaction
        //    var invoice = new Invoice
        //    {
        //        InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{request.UserId}",
        //        UserId = request.UserId,
        //        TotalAmount = request.Amount,
        //        IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
        //        Currency = "VND"
        //    };
        //    _transactionRepository.CreateInvoice(invoice);

        //    var tx = new Transaction
        //    {
        //        TransactionDate = DateTime.UtcNow,
        //        Amount = request.Amount,
        //        Currency = "VND",
        //        Status = "pending",
        //        Invoice = invoice
        //    };
        //    _transactionRepository.CreateTransaction(tx);
        //    _transactionRepository.SaveChanges(); // cần ID

        //    // 2) clear or mark carts later after success. For now keep them.

        //    if (string.Equals(request.PaymentMethod, "vnpay", StringComparison.OrdinalIgnoreCase))
        //    {
        //        var url = CreateVnPayUrl(request, tx);
        //        return new PaymentCreateResponse { PaymentUrl = url, TransactionRef = tx.TransactionId.ToString() };
        //    }
        //    else if (string.Equals(request.PaymentMethod, "momo", StringComparison.OrdinalIgnoreCase))
        //    {
        //        var url = CreateMomoUrl(request, tx);
        //        return new PaymentCreateResponse { PaymentUrl = url, TransactionRef = tx.TransactionId.ToString() };
        //    }
        //    else throw new ArgumentException("Unsupported method");
        //}

        //private string CreateVnPayUrl(PaymentRequest req, Transaction tx)
        //{
        //    var vnp_OrderId = tx.TransactionId.ToString();
        //    var vnp_Amount = ((long)req.Amount * 100).ToString(); // VNP: amount *100
        //    var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");

        //    var data = new SortedDictionary<string, string>();
        //    data["vnp_Version"] = "2.1.0";
        //    data["vnp_Command"] = "pay";
        //    data["vnp_TmnCode"] = vnp_TmnCode;
        //    data["vnp_Amount"] = vnp_Amount;
        //    data["vnp_CreateDate"] = vnp_CreateDate;
        //    data["vnp_CurrCode"] = "VND";
        //    data["vnp_IpAddr"] = "127.0.0.1";
        //    data["vnp_Locale"] = "vn";
        //    data["vnp_OrderType"] = "other";
        //    data["vnp_ReturnUrl"] = vnp_Returnurl;
        //    data["vnp_TxnRef"] = vnp_OrderId;

        //    var raw = string.Join("&", data.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));
        //    var sign = HmacSHA512(vnp_HashSecret, raw);
        //    var url = $"{vnp_Url}?{raw}&vnp_SecureHash={sign}";
        //    return url;
        }

        private string CreateMomoUrl(PaymentRequest req, Transaction tx)
        {
            var orderId = tx.TransactionId.ToString();
            var amount = ((long)req.Amount).ToString(); // integer
            var requestId = Guid.NewGuid().ToString();
            var ipnUrl = momo_Returnurl;
            var redirectUrl = momo_Returnurl;

            var rawHash = $"accessKey={momo_AccessKey}&amount={amount}&extraData=&ipnUrl={ipnUrl}&orderId={orderId}&partnerCode={momo_PartnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType=captureWallet";
            var signature = HmacSHA256(momo_SecretKey, rawHash);

            var body = new
            {
                partnerCode = momo_PartnerCode,
                accessKey = momo_AccessKey,
                requestId,
                amount,
                orderId,
                redirectUrl,
                ipnUrl,
                requestType = "captureWallet",
                extraData = "",
                signature
            };

            var json = JsonConvert.SerializeObject(body);
            var resp = _http.PostAsync(momo_Endpoint, new StringContent(json, Encoding.UTF8, "application/json")).Result;
            var respStr = resp.Content.ReadAsStringAsync().Result;
            dynamic d = JsonConvert.DeserializeObject(respStr);
            string payUrl = d.payUrl;
            return payUrl;
        }

        public bool HandleCallbackVnPay(IDictionary<string, string> query)
        {
            if (!query.ContainsKey("vnp_SecureHash")) return false;
            var secureHash = query["vnp_SecureHash"];
            // Build raw data (exclude vnp_SecureHash)
            var keys = query.Keys.Where(k => k.StartsWith("vnp_") && k != "vnp_SecureHash").OrderBy(k => k);
            var raw = string.Join("&", keys.Select(k => $"{k}={query[k]}"));
            var sign = HmacSHA512(vnp_HashSecret, raw);
            if (!string.Equals(sign, secureHash, StringComparison.OrdinalIgnoreCase)) return false;

            if (!query.ContainsKey("vnp_TxnRef")) return false;
            var txnRef = int.Parse(query["vnp_TxnRef"]);
            var tx = _transactionRepository.GetTransactionById(txnRef);
            if (tx == null) return false;

            var rsp = query.ContainsKey("vnp_ResponseCode") ? query["vnp_ResponseCode"] : "99";
            if (rsp == "00")
            {
                tx.Status = "completed";
                tx.TransactionDate = DateTime.UtcNow;
                _transactionRepository.UpdateTransaction(tx);
                _transactionRepository.SaveChanges();

                // clear cart items if needed
                return true;
            }
            else
            {
                tx.Status = "failed";
                _transactionRepository.UpdateTransaction(tx);
                _transactionRepository.SaveChanges();
                return false;
            }
        }

        public bool HandleCallbackMomo(Dictionary<string, string> body)
        {
            if (!body.ContainsKey("signature")) return false;
            var signature = body["signature"];
            // Build raw string per MoMo docs — here assume order of fields as used when create
            var rawString = $"accessKey={momo_AccessKey}&amount={body.GetValueOrDefault("amount", "")}&extraData={body.GetValueOrDefault("extraData", "")}&message={body.GetValueOrDefault("message", "")}&orderId={body.GetValueOrDefault("orderId", "")}&orderType={body.GetValueOrDefault("orderType", "")}&partnerCode={momo_PartnerCode}&payType={body.GetValueOrDefault("payType", "")}&requestId={body.GetValueOrDefault("requestId", "")}&responseTime={body.GetValueOrDefault("responseTime", "")}&result={body.GetValueOrDefault("result", "")}&transId={body.GetValueOrDefault("transId", "")}";
            var sign = HmacSHA256(momo_SecretKey, rawString);
            if (!string.Equals(sign, signature, StringComparison.OrdinalIgnoreCase)) return false;

            var orderId = int.Parse(body.GetValueOrDefault("orderId", "0"));
            var result = body.GetValueOrDefault("errorCode", "-1"); // 0 success
            var tx = _transactionRepository.GetTransactionById(orderId);
            if (tx == null) return false;

            if (result == "0")
            {
                tx.Status = "completed";
                _transactionRepository.UpdateTransaction(tx);
                _transactionRepository.SaveChanges();
                return true;
            }
            else
            {
                tx.Status = "failed";
                _transactionRepository.UpdateTransaction(tx);
                _transactionRepository.SaveChanges();
                return false;
            }
        }

        private static string HmacSHA512(string key, string input)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        private static string HmacSHA256(string key, string input)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(key)))
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
