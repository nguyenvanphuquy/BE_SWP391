using System.Security.Cryptography.Pkcs;

namespace BE_SWP391.Models.DTOs.Response
{
    public class OrderDetailResponse
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string Organization { get; set; }

        public string InvoiceName { get; set; }
        public DateOnly? IssueDay { get; set; }
        public string? MethodName { get; set; }
        public string Status { get; set; }
        public string PackageName { get; set; }
        public int Quantity { get; set; }
        public decimal? PackagePrice { get; set; }
        public decimal ? TotalPrice { get; set; }
        public decimal? SumPrice { get; set; }

    }
}
