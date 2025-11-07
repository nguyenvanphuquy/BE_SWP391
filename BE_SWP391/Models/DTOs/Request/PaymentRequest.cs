namespace BE_SWP391.Models.DTOs.Request
{
    public class PaymentRequest
    {
        public int UserId { get; set; }
        public int[] CartIds { get; set; }   // hoặc plan ids
        public decimal Amount { get; set; } // VND
        public string PaymentMethod { get; set; } // "vnpay" or "momo"
        public string OrderInfo { get; set; }
    }
}
