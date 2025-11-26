using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Common;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;

namespace BE_SWP391.Repositories.Implementations
{
    public class DataPackageRepository : IDataPackageRepository
    {
        private readonly EvMarketContext _context;
        public DataPackageRepository(EvMarketContext context)
        {
            _context = context;
        }
        public DataPackage? GetById(int id)
        {
            return _context.DataPackages.Find(id);
        }
        public IEnumerable<DataPackage> GetAll()
        {
            return _context.DataPackages.ToList();
        }
        public void Create(DataPackage dataPackage)
        {
            _context.DataPackages.Add(dataPackage);
            _context.SaveChanges();
        }
        public void Update(DataPackage dataPackage)
        {
            _context.DataPackages.Update(dataPackage);
            _context.SaveChanges();
        }
        public void Delete(DataPackage dataPackage)
        {
            _context.DataPackages.Remove(dataPackage);
            _context.SaveChanges();
        }

        public int CountPending()
        {
            return _context.DataPackages.Count(x => x.Status == "Pending");
        }
        public int CountApproved()
        {
            return _context.DataPackages.Count(x => x.Status == "Approved");
        }
        public int CountRejected()
        {
            return _context.DataPackages.Count(x => x.Status == "Rejected");
        }
        public List<DataForAdminResponse> GetDataForAdmin()
        {
            var data = (from p in _context.DataPackages
                        join u in _context.Users on p.UserId equals u.UserId
                        join s in _context.SubCategorys on p.SubcategoryId equals s.SubcategoryId
                        join m in _context.MetaDatas on p.MetadataId equals m.MetadataId
                        orderby m.CreatedAt descending
                        select new DataForAdminResponse
                        {
                            PackageId = p.PackageId,
                            PackageName = p.PackageName,
                            ProviderName = u.FullName,
                            CategoryName = s.SubcategoryName,
                            FileSize = m.FileSize,
                            CreatedAt = m.CreatedAt,
                            Status = p.Status,
                        }).ToList();
            return data;
        }
        public void ChageStatus(DataPackage dataPackage)
        {
            _context.DataPackages.Update(dataPackage);
            _context.SaveChanges();
        }
        public List<DataPendingRepsonse> GetDataPending()
        {
            var result = (from p in _context.DataPackages
                          join u in _context.Users on p.UserId equals u.UserId
                          where p.Status == "Pending"
                          orderby p.ReleaseDate descending
                          select new DataPendingRepsonse
                          {
                              PackageName = p.PackageName,
                              ProviderName = u.FullName,
                              CreateAt = p.ReleaseDate
                          })
                          .ToList();
            return result;

        }
        public List<AllPackageResponse> GetAllPackage()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var data = (from dp in _context.DataPackages
                        join u in _context.Users on dp.UserId equals u.UserId
                        join mt in _context.MetaDatas on dp.MetadataId equals mt.MetadataId
                        join pp in _context.PricingPlans on dp.PackageId equals pp.PackageId
                        where dp.Status == "Approved"
                        select new AllPackageResponse
                        {
                            PrincingPlanId = pp.PlanId,
                            PackageName = dp.PackageName,
                            ProviderName = u.FullName,
                            Type = mt.Type,
                            Rating = Math.Round(
                                        (from fb in _context.Feedbacks
                                         where fb.PackageId == dp.PackageId
                                         select (double?)fb.Rating).Average() ?? 0, 1),
                            DownloadCount = _context.Downloads.Count(i => i.PackageId == dp.PackageId),
                            CartCount = _context.Carts.Count(c => c.PlanId == pp.PlanId),
                            CreateAt = dp.ReleaseDate,
                            UpdateAt = dp.LastUpdate,
                            Description = dp.Description,
                            FileFormat = mt.FileFormat,
                            FileSize = mt.FileSize,
                            Version = dp.Version,
                            PricingPlan = pp.Price,
                        })
                        .OrderByDescending(i => i.DownloadCount)
                        .ToList();
            return data;
        }
        public UserDataStatsResponse GetUserDataStats(int userId)
        {
            var query = _context.DataPackages.Where(dp => dp.UserId == userId);
            var totalData = query.Count();
            var active = query.Count(dp => dp.Status == "Active");
            var approved = query.Count(dp => dp.Status == "Approved");
            var pending = query.Count(dp => dp.Status == "Pending");
            return new UserDataStatsResponse
            {
                TotalData = totalData,
                ActiveData = active,
                ApprovedData = approved,
                PendingData = pending,
            };
        }
        public List<UserDataResponse> GetUserDataByUserId(int userId)
        {
            var data = (from dp in _context.DataPackages
                        join sc in _context.SubCategorys on dp.SubcategoryId equals sc.SubcategoryId
                        join mt in _context.MetaDatas on dp.MetadataId equals mt.MetadataId into metadataGroup
                        from mt in metadataGroup.DefaultIfEmpty()
                        join pp in _context.PricingPlans on dp.PackageId equals pp.PackageId into pricingGroup
                        from pp in pricingGroup.DefaultIfEmpty()
                        orderby dp.ReleaseDate descending
                        where dp.UserId == userId
                        select new UserDataResponse
                        {
                            PackageId = dp.PackageId,
                            PackageName = dp.PackageName,
                            Description = dp.Description,
                            FileSize = mt.FileSize,
                            SubCategoryName = sc.SubcategoryName,
                            status = dp.Status,
                            DownloadCount = _context.Downloads.Count(i => i.PackageId == dp.PackageId),
                            Price = pp.Price,

                        }
                        ).ToList();
            return data;
        }
        public List<DataForUserResponse> GetDataForUser(int userId)
        {
            var purchasedPackages = (from inv in _context.Invoices
                                     join t in _context.Transactions on inv.InvoiceId equals t.InvoiceId
                                     join pp in _context.PricingPlans on t.PlanId equals pp.PlanId into ppGroup
                                     from pp in ppGroup.DefaultIfEmpty()
                                     join dp in _context.DataPackages on pp.PackageId equals dp.PackageId into dpGroup
                                     from dp in dpGroup.DefaultIfEmpty()
                                     join u in _context.Users on dp.UserId equals u.UserId into uGroup
                                     from u in uGroup.DefaultIfEmpty()
                                     join mt in _context.MetaDatas on dp.MetadataId equals mt.MetadataId into mtGroup
                                     from mt in mtGroup.DefaultIfEmpty()
                                     where inv.UserId == userId && t.Status == "completed"
                                     select new
                                     {
                                         t.TransactionId,
                                         dp.PackageId,
                                         dp.PackageName,
                                         ProviderName = u.FullName,
                                         t.TransactionDate,
                                         mt.FileFormat,
                                         mt.FileSize
                                     })
                                    .Distinct()
                                    .ToList();

            var packageIds = purchasedPackages.Select(p => p.PackageId).ToList();

            var allDownloads = _context.Downloads
                .Where(d => packageIds.Contains(d.PackageId))
                .OrderByDescending(d => d.DownloadDate)
                .Select(d => new DownloadInfo
                {
                    DownloadId = d.DownloadId,
                    DownloadDate = d.DownloadDate,
                    FileUrl = d.FileUrl,
                    FileName = System.IO.Path.GetFileName(d.FileUrl),
                    Status = d.Status,
                    DownloadCount = d.DownloadCount,
                    PackageId = d.PackageId
                })
                .ToList();

            var downloadsByPackage = allDownloads
                .GroupBy(d => d.PackageId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = purchasedPackages.Select(p =>
            {
                if (downloadsByPackage.TryGetValue(p.PackageId, out var downloads))
                {
                    return new DataForUserResponse
                    {
                        TransactionId = p.TransactionId,
                        PackageId = p.PackageId,
                        PackageName = p.PackageName,
                        ProviderName = p.ProviderName,
                        PurchaseDate = p.TransactionDate,
                        FileFormat = p.FileFormat,
                        FileSize = p.FileSize,
                        TotalDownloads = downloads.Count,
                        LatestDownloadDate = downloads.Any() ? downloads.Max(d => d.DownloadDate) : (DateTime?)null,
                        Status = "Purchased",
                        Downloads = downloads
                    };
                }
                else
                {
                    return new DataForUserResponse
                    {
                        TransactionId = p.TransactionId,
                        PackageId = p.PackageId,
                        PackageName = p.PackageName,
                        ProviderName = p.ProviderName,
                        PurchaseDate = p.TransactionDate,
                        FileFormat = p.FileFormat,
                        FileSize = p.FileSize,
                        TotalDownloads = 0,
                        LatestDownloadDate = null,
                        Status = "Purchased",
                        Downloads = new List<DownloadInfo>()
                    };
                }
            }).ToList();

            return result;
        }
        public ReportOrderResponse GetReportOrder(int userId)
        {
            var userDataPackages = (
                from inv in _context.Invoices
                join u in _context.Users on inv.UserId equals u.UserId
                join t in _context.Transactions on inv.InvoiceId equals t.InvoiceId
                join pp in _context.PricingPlans on t.PlanId equals pp.PlanId
                join dp in _context.DataPackages on pp.PackageId equals dp.PackageId
                where inv.UserId == userId
                select dp
            ).Distinct()
            .ToList();

            var totalPackages = userDataPackages.Count();

            var thisMonth = DateTime.Now.Month;
            var newPackagesThisMonth = userDataPackages
                .Count(dp => dp.ReleaseDate != null && dp.ReleaseDate.Value.Month == thisMonth);

            var packageIds = userDataPackages.Select(dp => dp.PackageId).ToList();

            var downloads = _context.Downloads
                .Where(d => packageIds.Contains(d.PackageId))
                .ToList();

            var totalDownloads = downloads.Count();
            var totalRemaining = totalPackages * 10 - totalDownloads;

            var activeCount = downloads.Count(d => d.Status == "Active");
            var expiredCount = downloads.Count(d => d.Status == "Expired");

            return new ReportOrderResponse
            {
                TotalDownload = totalDownloads,
                TotalRemaining = totalRemaining,
                TotalPackage = totalPackages,
                NewPackageThisMonth = newPackagesThisMonth,
                ActiveCount = activeCount,
                ExpiredCount = expiredCount
            };
        }
        public PackageDetailResponse GetPackageDetails(int packageId)
        {
            var packageDetail = (from dp in _context.DataPackages
                                 join u in _context.Users on dp.UserId equals u.UserId
                                 join mt in _context.MetaDatas on dp.MetadataId equals mt.MetadataId
                                 join sc in _context.SubCategorys on dp.SubcategoryId equals sc.SubcategoryId
                                 join pp in _context.PricingPlans on dp.PackageId equals pp.PackageId
                                 where dp.PackageId == packageId
                                 select new PackageDetailResponse
                                 {
                                     PackageName = dp.PackageName,
                                     ProviderName = u.FullName,
                                     SubCategoryName = sc.SubcategoryName,
                                     FileSize = mt.FileSize,
                                     Price = pp.Price,
                                     Duration = pp.Duration,
                                     FileCount = _context.Downloads.Count(d => d.PackageId == dp.PackageId),
                                     Status = dp.Status,
                                     ReleaseDate = dp.ReleaseDate,
                                     PackageId = dp.PackageId
                                 })
                                .FirstOrDefault();

            if (packageDetail == null)
            {
                return null;
            }

            // Lấy danh sách downloads cho package này
            var downloads = _context.Downloads
                .Where(d => d.PackageId == packageId)
                .OrderByDescending(d => d.DownloadDate)
                .Select(d => new DownloadInfo
                {
                    DownloadId = d.DownloadId,
                    DownloadDate = d.DownloadDate,
                    FileUrl = d.FileUrl,
                    FileName = System.IO.Path.GetFileName(d.FileUrl),
                    Status = d.Status,
                    DownloadCount = d.DownloadCount,
                    PackageId = d.PackageId
                })
                .ToList();

            packageDetail.Downloads = downloads;

            return packageDetail;
        }
    }
}