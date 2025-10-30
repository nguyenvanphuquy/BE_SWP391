namespace BE_SWP391.Models.DTOs.Request
{
    public class TransactionRequest
    {
        public DateTime? TransactionDate { get; set; }

        public string? Status { get; set; }

        public decimal? Amount { get; set; }

        public string? Currency { get; set; }

        public int InvoiceId { get; set; }
    }
}
