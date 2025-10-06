namespace BE_SWP391.Models.DTOs.Response
{
    public class TransactionResponse
    {
        public int TransactionId { get; set; }
        public DateTime? TransactionDate { get; set; }

        public string? Status { get; set; }

        public decimal? Amount { get; set; }

        public string? Currency { get; set; }

        public int? InvoiceId { get; set; }
    }
}
