namespace BE_SWP391.Models.DTOs.Response
{
    public class DashboardStatsResponse
    {
        public decimal TotalRevenue { get; set; }
        public decimal PlatforCommission { get; set; } 
        public decimal ProviderShare { get; set; }
        public int TotalTransactions { get; set; }

    }
}
