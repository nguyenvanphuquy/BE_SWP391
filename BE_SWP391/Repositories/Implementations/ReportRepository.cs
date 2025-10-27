﻿using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

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
        public List<TopPackageResponse> GetTopDownloadedPackages(int top = 10)
        {
            var currentMonth = DateTime.Now.Month;
            var lastMonth = DateTime.Now.AddMonths(-1).Month;

            var data = (from dp in _marketContext.DataPackages
                        join d in _marketContext.Downloads on dp.PackageId equals d.PackageId
                        join u in _marketContext.Users on dp.UserId equals u.UserId
                        join sc in _marketContext.SubCategorys on dp.SubcategoryId equals sc.SubcategoryId
                        join cat in _marketContext.Categorys on sc.CategoryId equals cat.CategoryId
                        group new { dp, d, u, cat } by new
                        {
                            dp.PackageId,
                            dp.PackageName,
                            u.FullName,
                            cat.CategoryName
                        } into g
                        orderby g.Count() descending
                        select new TopPackageResponse
                        {
                            PackageName = g.Key.PackageName,
                            TotalDownloads = g.Count(),
                            ProviderName = g.Key.FullName,
                            CategoryName = g.Key.CategoryName,
                        })
                        .Take(top)
                        .ToList();

            return data;
        }
        public List<TopProviderResponse> GetTopProviders(int top)
        {
            var providers =
                (from user in _marketContext.Users
                 where user.RoleId == 2
                 select new TopProviderResponse
                 {
                     ProviderName = user.FullName,

                     TotalPackages = _marketContext.DataPackages
                         .Count(dp => dp.UserId == user.UserId),

                     Rating = _marketContext.Feedbacks
                         .Where(f => f.Package.UserId == user.UserId)
                         .Average(f => (double?)f.Rating) ?? 0,

                     TotalRevenue = _marketContext.RevenueShares
                         .Where(rs => rs.UserId == user.UserId)
                         .Sum(rs => (double?)rs.Amount) ?? 0
                 })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(top)
                .ToList();

            return providers;
        }
        public List<CategoryAnalyticsResponse> GetCategoryAnalytics()
        {
            var data = (from cat in _marketContext.Categorys
                        select new CategoryAnalyticsResponse
                        {
                            CategoryName = cat.CategoryName,

                            TotalPackages = _marketContext.DataPackages
                                .Where(dp => dp.Subcategory.CategoryId == cat.CategoryId)
                                .Count(),

                            TotalDownloads = _marketContext.Downloads
                                .Where(d => d.Package.Subcategory.CategoryId == cat.CategoryId)
                                .Count(),

                            TotalRevenue = (from rs in _marketContext.RevenueShares
                                            join t in _marketContext.Transactions on rs.TransactionId equals t.TransactionId
                                            join inv in _marketContext.Invoices on t.InvoiceId equals inv.InvoiceId
                                            join dp in _marketContext.DataPackages on inv.UserId equals dp.UserId
                                            where dp.Subcategory.CategoryId == cat.CategoryId
                                            select rs.Amount
                                   ).Sum(x => (double?)x) ?? 0
                        })
                .OrderByDescending(x => x.TotalRevenue)
                        .ToList();

            return data;
        }
        public DashboardSummaryResponse GetDashboardSummary()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var result = new DashboardSummaryResponse
            {
                TotalUsers = _marketContext.Users.Count(),

                TotalDataPackages = _marketContext.DataPackages.Count(),

                MonthlyRevenue = _marketContext.RevenueShares
                    .Where(rs => rs.DistributedAt.HasValue &&
                                 rs.DistributedAt.Value.Month == currentMonth &&
                                 rs.DistributedAt.Value.Year == currentYear)
                    .Sum(rs => (double?)rs.Amount) ?? 0,

                TotalTransactions = _marketContext.Transactions.Count()
            };

            return result;
        }
    }
}
