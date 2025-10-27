using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface IReportService
    {
        DashboardStatsResponse DashboardStats();
        AnalyticsResponse GetTotal();
    }
}
