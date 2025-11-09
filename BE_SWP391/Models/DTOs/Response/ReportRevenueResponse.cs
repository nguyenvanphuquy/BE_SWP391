namespace BE_SWP391.Models.DTOs.Response
{
    public class ReportRevenueResponse
    {
        public int TotalRevenue { get; set; }
        public int DownloadCount { get; set; }
        public int BuyerCount { get; set; }
        public int AverageRevenue { get; set; }
    }
}
