using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IReportRepository
    {
        decimal GetTotalRevenue();
        decimal GetTotalCommission();
        int GetTotalTransacton();
        AnalyticsResponse GetTotal();
    }
}
