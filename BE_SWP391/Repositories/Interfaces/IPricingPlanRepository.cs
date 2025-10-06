using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface IPricingPlanRepository
    {
        PricingPlan? GetById(int id);
        IEnumerable<PricingPlan> GetAll();
        void Create(PricingPlan pricingPlan);
        void Update(PricingPlan pricingPlan);
        void Delete(PricingPlan pricingPlan);
    }
}
