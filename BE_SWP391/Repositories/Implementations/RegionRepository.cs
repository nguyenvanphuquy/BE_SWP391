using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models;
using BE_SWP391.Repositories.Interfaces;
namespace BE_SWP391.Repositories.Implementations
{
    public class RegionRepository : IRegionRepository
    {
        private readonly EvMarketContext _context;
        public RegionRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Region? GetById(int id)
        {
            return _context.Regions.Find(id);
        }
        public IEnumerable<Region> GetAll()
        {
            return _context.Regions.ToList();
        }
        public void Create(Region region)
        {
            _context.Regions.Add(region);
            _context.SaveChanges();
        }
        public void Update(Region region)
        {
            _context.Regions.Update(region);
            _context.SaveChanges();
        }
        public void Delete(Region region)
        {
                _context.Regions.Remove(region);
                _context.SaveChanges();
        }


    }
}
