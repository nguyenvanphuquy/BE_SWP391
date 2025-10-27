namespace BE_SWP391.Models.DTOs.Response
{
    public class DashboardSummaryResponse
    {
        public int TotalUsers { get; set; }
        public int TotalDataPackages { get; set; }
        public double MonthlyRevenue { get; set; }
        public int TotalTransactions { get; set; }
    }
}
