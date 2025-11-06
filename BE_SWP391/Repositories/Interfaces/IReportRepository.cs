using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IReportRepository
    {
        decimal GetTotalRevenue();
        decimal GetTotalCommission();
        int GetTotalTransacton();
        AnalyticsResponse GetTotal();
        List<TopPackageResponse> GetTopDownloadedPackages(int top = 10);
        List<TopProviderResponse> GetTopProviders(int top);
        List<CategoryAnalyticsResponse> GetCategoryAnalytics();
        DashboardSummaryResponse GetDashboardSummary();
        OrderReportResponse GetOrderReport(int userId);
        List<OrderListResponse> GetOrderList(int userId);

    }
}
