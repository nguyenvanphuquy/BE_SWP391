using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models;
using BE_SWP391.Repositories.Interfaces;

namespace BE_SWP391.Repositories.Implementations
{
    public class RevenueShareRepository : IRevenueShareRepository
    {
        private readonly EvMarketContext _context;
        public RevenueShareRepository(EvMarketContext context)
        {
            _context = context;
        }
        public RevenueShare? GetById(int id)
        {
            return _context.RevenueShares.Find(id);
        }
        public IEnumerable<RevenueShare> GetAll()
        {
            return _context.RevenueShares.ToList();
        }
        public void Create(RevenueShare revenueShare)
        {
            _context.RevenueShares.Add(revenueShare);
            _context.SaveChanges();
        }
        public void Update(RevenueShare revenueShare)
        {
            _context.RevenueShares.Update(revenueShare);
            _context.SaveChanges();
        }
        public void Delete(RevenueShare revenueShare)
        {
                _context.RevenueShares.Remove(revenueShare);
                _context.SaveChanges();
        }
    }
}
