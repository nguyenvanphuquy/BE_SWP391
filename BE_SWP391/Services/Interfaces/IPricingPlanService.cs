using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface IPricingPlanService
    {
        PricingPlanResponse? GetById(int id);
        IEnumerable<PricingPlanResponse> GetAll();
        PricingPlanResponse? Create(PricingPlanRequest request);
        PricingPlanResponse? Update(int id, PricingPlanRequest request);
        bool Delete(int id);
        ReportPricingStaffResponse GetReportPricingStaff(int userId);
    }
}
