using BE_SWP391.Data;
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
            return _context.DataPackages.ToList().Select(ToResponse);
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
        public static DataPackage ToResponse(DataPackage dataPackage)
        {
            return new DataPackage
            {
                PackageId = dataPackage.PackageId,
                PackageName = dataPackage.PackageName,
                Description = dataPackage.Description,
                Version = dataPackage.Version,
                ReleaseDate = dataPackage.ReleaseDate,
                LastUpdate = dataPackage.LastUpdate,
                Status = dataPackage.Status,
                UserId = dataPackage.UserId,
                SubcategoryId = dataPackage.SubcategoryId,
                MetadataId = dataPackage.MetadataId
            };
        }
    }
}
