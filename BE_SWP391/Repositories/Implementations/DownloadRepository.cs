using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_SWP391.Repositories.Implementations
{
    public class DownloadRepository : IDownloadRepository
    {
        private readonly EvMarketContext _context;
        public DownloadRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Download GetById(int id)
        {
            return _context.Downloads
                .Include(d => d.Package)
                .FirstOrDefault(d => d.DownloadId == id);
        }

        public IEnumerable<Download> GetAll()
        {
            return _context.Downloads
                .Include(d => d.Package)
                .ToList();
        }

        public IEnumerable<Download> GetByPackageId(int packageId)
        {
            return _context.Downloads
                .Where(d => d.PackageId == packageId)
                .Include(d => d.Package)
                .ToList();
        }

        public void Create(Download download)
        {
            _context.Downloads.Add(download);
            _context.SaveChanges();
        }

        public void Update(Download download)
        {
            _context.Downloads.Update(download);
            _context.SaveChanges();
        }

        public void Delete(Download download)
        {
            _context.Downloads.Remove(download);
            _context.SaveChanges();
        }
    }
}
