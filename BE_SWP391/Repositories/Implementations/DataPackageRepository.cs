using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;

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
                        join mt in _context.MetaDatas on dp.MetadataId equals mt.MetadataId
                        //from plan in planGroup.DefaultIfEmpty()
                        where dp.UserId == userId
                        select new UserDataResponse
                        {
                            PackageName = dp.PackageName,
                            Description = dp.Description,
                            FileSize = mt.FileSize,
                            status = dp.Status,
                            DownloadCount = _context.Downloads.Count(i => i.PackageId == dp.PackageId),
                            RevenueCount = _context.RevenueShares.Sum(j => j.Amount)
                        }
                        ).ToList();
            return data;
        }
        public List<DataForUserResponse> GetDataForUser(int userId)
        {
            var data = (from dp in _context.DataPackages
                        join u in _context.Users on dp.UserId equals u.UserId
                        join mt in _context.MetaDatas on dp.MetadataId equals mt.MetadataId
                        join pp in _context.PricingPlans on dp.PackageId equals pp.PackageId
                        join t in _context.Transactions on pp.TransactionId equals t.TransactionId
                        where dp.UserId == userId
                        select new DataForUserResponse
                        {
                            PackageId = dp.PackageId,
                            PackageName = dp.PackageName,
                            ProviderName = u.FullName,
                            PurchaseDate = t.TransactionDate,
                            FileFormat = mt.FileFormat,
                            FileSize = mt.FileSize,
                            Status = dp.Status

                        })
                        .ToList();
            return data;
        }
    }
}
