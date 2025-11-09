namespace BE_SWP391.Models.DTOs.Response
{
    public class TransactionNowResponse
    {
        public DateTime? TransactionDate { get; set; }
        public string BuyerName { get; set; }
        public string PackageName { get; set; }
        public int? DownloadCount { get; set; }
        public decimal? Revenue { get; set; }
    }
}
