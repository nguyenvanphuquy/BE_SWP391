namespace BE_SWP391.Models.DTOs.Request
{
    public class PaymentMethodRequest
    {
        public string? MethodName { get; set; }

        public string? Provider { get; set; }

        public string? Details { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int TransactionId { get; set; }

    }
}
