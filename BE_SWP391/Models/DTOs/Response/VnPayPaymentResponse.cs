namespace BE_SWP391.Models.DTOs.Response
{
    public class VnPayPaymentResponse
    {
        public bool Success { get; set; }
        public string PaymentUrl { get; set; }
        public int TransactionId { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
    }
}
