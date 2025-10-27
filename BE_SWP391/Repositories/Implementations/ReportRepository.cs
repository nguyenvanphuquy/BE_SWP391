using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Repositories.Interfaces;

namespace BE_SWP391.Repositories.Implementations
{
    public class ReportRepository: IReportRepository
    {
        private readonly EvMarketContext _marketContext;
        public ReportRepository(EvMarketContext marketContext)
        {
            _marketContext = marketContext;
        }
        public decimal GetTotalRevenue()
        {
            return _marketContext.Invoices.Sum(i => i.TotalAmount.GetValueOrDefault());
        }
        public decimal GetTotalCommission()
        {
            return _marketContext.RevenueShares.Sum(i => i.Amount);
        }
        public int GetTotalTransacton()
        {
            return _marketContext.Transactions.Count();
        }
        public AnalyticsResponse GetTotal()
        {
            var totalData = _marketContext.DataPackages.Count();
            var totalDownload = _marketContext.Downloads.Count();
            var totalTransaction = _marketContext.Transactions.Count();
            var totalRevenue = _marketContext.RevenueShares.Sum(i => i.Amount);
            return new AnalyticsResponse
            {
                TotalDataPackage = totalData,
                TotalDownLoad = totalDownload,
                TotalTransaction = totalTransaction,
                TotalRevenue = totalRevenue
            };
        }
    }
}
