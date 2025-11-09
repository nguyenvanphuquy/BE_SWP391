namespace BE_SWP391.Models.DTOs.Response
{
    public class DataRevenueResponse
    {
        public string PackageName { get; set; }
        public int TotalDownloads { get; set; }
        public decimal? TotalRevenue { get; set; }
        public decimal? AveragePrice { get; set; }
    }
}
