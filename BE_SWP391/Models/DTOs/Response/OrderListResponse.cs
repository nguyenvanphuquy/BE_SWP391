namespace BE_SWP391.Models.DTOs.Response
{
    public class OrderListResponse
    {
        public int InvoiceId { get; set; }  
        public string InvoiceName { get; set; }
        public DateOnly? IssueDay { get; set; }
        public int PackageCount { get; set; }
        public decimal? SumPrice { get; set; }
        public string MethodName { get; set; }
        public string Status { get; set; }

    }
}
