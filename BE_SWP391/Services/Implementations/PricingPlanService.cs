using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models.DTOs.Request;

namespace BE_SWP391.Services.Implementations
{
    public class PricingPlanService : IPricingPlanService
    {
        private readonly IPricingPlanRepository _pricingPlanRepository;
        public PricingPlanService(IPricingPlanRepository pricingPlanRepository)
        {
            _pricingPlanRepository = pricingPlanRepository;
        }
        public PricingPlanResponse? GetById(int id)
        {
            var pricingPlan = _pricingPlanRepository.GetById(id);
            return pricingPlan == null ? null : ToResponse(pricingPlan);
        }
        public IEnumerable<PricingPlanResponse> GetAll()
        {
            return _pricingPlanRepository.GetAll().Select(ToResponse);
        }
        public PricingPlanResponse? Create(PricingPlanRequest request)
        {
            var pricingPlan = new PricingPlan
            {
                PlanName = request.PlanName,
                Price = request.Price,
                Currency = request.Currency,
                Duration = request.Duration,
                AccessType = request.AccessType,
                PackageId = request.PackageId,
                Discount = request.Discount
            };
            _pricingPlanRepository.Create(pricingPlan);
            return ToResponse(pricingPlan);
        }
        public PricingPlanResponse? Update(int id, PricingPlanRequest request)
        {
            var pricingPlan = _pricingPlanRepository.GetById(id);
            if (pricingPlan == null) return null;

            pricingPlan.PlanName = request.PlanName;
            pricingPlan.Price = request.Price;
            pricingPlan.Currency = request.Currency;
            pricingPlan.Duration = request.Duration;
            pricingPlan.AccessType = request.AccessType;
            pricingPlan.PackageId = request.PackageId;
            pricingPlan.Discount = request.Discount;
            _pricingPlanRepository.Update(pricingPlan);
            return ToResponse(pricingPlan);
        }
        public bool Delete(int id)
        {
            var pricingPlan = _pricingPlanRepository.GetById(id);
            if (pricingPlan == null) return false;
            _pricingPlanRepository.Delete(pricingPlan);
            return true;
        }
        public static PricingPlanResponse? ToResponse(PricingPlan pricingPlan)
        {
            return new PricingPlanResponse
            {
                PlanId = pricingPlan.PlanId,
                PlanName = pricingPlan.PlanName,
                Price = pricingPlan.Price,
                Currency = pricingPlan.Currency,
                Duration = pricingPlan.Duration,
                AccessType = pricingPlan.AccessType,
                PackageId = pricingPlan.PackageId,
                Discount = pricingPlan.Discount
            };

        }
        public ReportPricingStaffResponse GetReportPricingStaff(int userId)
        {
            return _pricingPlanRepository.GetReportPricingStaff(userId);
        }
    }
}
