namespace BE_SWP391.Models.DTOs.Response
{
    public class TransactionStatusResponse
    {
        public int TransactionId { get; set; }
        public string? Status { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentMethod { get; set; }
    }
}
