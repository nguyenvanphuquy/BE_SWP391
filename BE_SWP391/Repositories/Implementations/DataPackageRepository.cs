using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BE_SWP391.Repositories.Implementations
{
    public class DataPackageRepository : IDataPackageRepository
    {
        private readonly  EvMarketContext _context;
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
                            PackageName = dp.PackageName,
                            ProviderName = u.FullName,
                            Type = mt.Type,
                            Rating = Math.Round(
                                        (from fb in _context.Feedbacks
                                         where fb.PackageId == dp.PackageId
                                         select (double?)fb.Rating).Average() ?? 0, 1),
                            DownloadCount = _context.Downloads.Count(i => i.PackageId == dp.PackageId),
                            CreateAt = dp.ReleaseDate,
                            PricingPlan = pp.Price,
                        })
                        .OrderByDescending(i => i.DownloadCount)
                        .ToList();
            return data;
        }
    }
}
