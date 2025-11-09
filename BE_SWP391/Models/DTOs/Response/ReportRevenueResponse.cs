namespace BE_SWP391.Models.DTOs.Response
{
    public class ReportRevenueResponse
    {
        public decimal? TotalRevenue { get; set; }
        public decimal? RevenueGrowth { get; set; }

        public int DownloadCount { get; set; }
        public decimal? DownloadGrowth { get; set; }
        public int BuyerCount { get; set; }
        public int NewBuyer { get; set; }
        public decimal? AverageRevenue { get; set; }
    }
}
