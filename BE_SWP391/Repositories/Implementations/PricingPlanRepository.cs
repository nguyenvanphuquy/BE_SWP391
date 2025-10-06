using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models;
using BE_SWP391.Repositories.Interfaces;
namespace BE_SWP391.Repositories.Implementations
{
    public class PricingPlanRepository : IPricingPlanRepository
    {
        private readonly EvMarketContext _context;
        public PricingPlanRepository(EvMarketContext context)
        {
            _context = context;
        }
        public PricingPlan? GetById(int id)
        {
            return _context.PricingPlans.Find(id);
        }
        public IEnumerable<PricingPlan> GetAll()
        {
            return _context.PricingPlans.ToList();
        }
        public void Create(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Add(pricingPlan);
            _context.SaveChanges();
        }
        public void Update(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Update(pricingPlan);
            _context.SaveChanges();
        }
        public void Delete(PricingPlan pricingPlan)
        {
                _context.PricingPlans.Remove(pricingPlan);
                _context.SaveChanges();
        }
    }
}
