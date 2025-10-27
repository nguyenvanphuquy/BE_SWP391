namespace BE_SWP391.Models.DTOs.Response
{
    public class RecentTransactionResponse
    {
        public int TransactionId { get; set; }
        public string ProviderName { get; set; }
        public string PackageName { get; set; }
        public decimal? Amount { get; set; }
        public decimal Commission { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string Status { get; set; }

    }
}
