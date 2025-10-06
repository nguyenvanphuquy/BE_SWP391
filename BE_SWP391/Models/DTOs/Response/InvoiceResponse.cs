namespace BE_SWP391.Models.DTOs.Response
{
    public class InvoiceResponse
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = null!;

        public DateOnly? IssueDate { get; set; }

        public DateOnly? DueDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Currency { get; set; }

        public decimal? TaxAmount { get; set; }

        public int UserId { get; set; }

        public string? Status { get; set; }

    }
}
