using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface IReportService
    {
        DashboardStatsResponse DashboardStats();
        AnalyticsResponse GetTotal();
        List<TopPackageResponse> GetTopPackages(int top = 10);
        List<TopProviderResponse> GetTopProviders(int top);
        List<CategoryAnalyticsResponse> GetCategoryAnalytics();
        DashboardSummaryResponse GetDashboardSummary();
        OrderReportResponse GetOrderReport(int userId);
        List<OrderListResponse> GetOrderList(int userId);
        OrderDetailResponse GetOrderDetail(int invoiceId);
    }
}
