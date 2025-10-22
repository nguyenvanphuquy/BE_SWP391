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
    }
}
