using BE_SWP391.Data;
using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
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
        public ReportPricingStaffResponse GetReportPricingStaff(int userId)
        {
            var packages = (from dp in _context.DataPackages
                            join pp in _context.PricingPlans on dp.PackageId equals pp.PackageId
                            where dp.UserId == userId
                            select pp).ToList();

            var packageCount = packages.Count();
            if (packageCount == 0)
            {
                return new ReportPricingStaffResponse
                {
                    AvenragePricing = 0,
                    PackageCount = 0,
                    PricingPlan = 0
                };
            }
            var totalPricing = packages.Sum(pp => pp.Price);
            var averagePricing = totalPricing / packageCount;
            return new ReportPricingStaffResponse
            {
                AvenragePricing = averagePricing,
                PackageCount = packageCount,
                PricingPlan = packageCount
            };
        }
        public List<ListPricingResponse> GetListPricing(int userId)
        {
            var query = (from pp in _context.PricingPlans
                         join dp in _context.DataPackages on pp.PackageId equals dp.PackageId into dpGroup
                         from dp in dpGroup.DefaultIfEmpty()
                         where dp.UserId == userId
                         select new ListPricingResponse
                        {
                            PricingId = pp.PlanId,
                             PackageName = dp.PackageName,
                            Description = dp.Description,
                            Price = pp.Price,
                            status = dp.Status

                        } )
                        .ToList();
            return query;
        }
    }
}
