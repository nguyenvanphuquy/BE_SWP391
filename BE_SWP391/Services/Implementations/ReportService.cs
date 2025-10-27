using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Repositories.Implementations;
using BE_SWP391.Repositories.Interfaces;

namespace BE_SWP391.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }
        public DashboardStatsResponse DashboardStats()
        {
            var revenue =_repository.GetTotalRevenue();
            var commission = _repository.GetTotalCommission();
            var transaction = _repository.GetTotalTransacton();
            return new DashboardStatsResponse
            {
                TotalRevenue = revenue,
                PlatforCommission = commission,
                ProviderShare = revenue - commission,
                TotalTransactions = transaction
            };
        }
        public AnalyticsResponse GetTotal()
        {
            return _repository.GetTotal();
        }
    }
}
