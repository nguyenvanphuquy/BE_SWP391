namespace BE_SWP391.Models.DTOs.Response
{
    public class PaymentMethodResponse
    {
        public int MethodId { get; set; }
        public string? MethodName { get; set; }

        public string? Provider { get; set; }

        public string? Details { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int TransactionId { get; set; }

    }
}
