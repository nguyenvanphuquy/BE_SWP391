using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models.DTOs.Response;

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
        public List<ProfitResponse> GetProfit()
        {
            var profit = (from rs in _context.RevenueShares
                          join user in _context.Users on rs.UserId equals user.UserId
                          group new { rs, user } by new { rs.UserId, user.Username } into g
                          select new ProfitResponse
                          {
                              ProviderName = g.Key.Username,
                              SharePercentage = g.First().rs.SharePercentage,
                              PlatformPercentage = 100 - g.First().rs.SharePercentage
                          })
                           .ToList();
            return profit;
        }
    }
}
