namespace BE_SWP391.Models.DTOs.Response
{
    public class CategoryAnalyticsResponse
    {
        public string CategoryName { get; set; }
        public int TotalPackages { get; set; }
        public int TotalDownloads { get; set; }
        public double TotalRevenue { get; set; }
    }

}
