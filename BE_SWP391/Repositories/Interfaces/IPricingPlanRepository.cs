using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface IPricingPlanRepository
    {
        PricingPlan? GetById(int id);
        IEnumerable<PricingPlan> GetAll();
        PricingPlanResponse Create(PricingPlanRequest request);
        UpdatePricingResponse UpdatePricing(UpdatePricingRequest request);
        void Delete(PricingPlan pricingPlan);
        ReportPricingStaffResponse GetReportPricingStaff(int userId);
        List<ListPricingResponse> GetListPricing(int userId);
        ReportRevenueResponse GetRevenueReport(int userId);
    }
}
