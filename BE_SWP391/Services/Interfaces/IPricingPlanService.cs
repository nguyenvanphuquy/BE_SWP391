using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface IPricingPlanService
    {
        PricingPlanResponse? GetById(int id);
        IEnumerable<PricingPlanResponse> GetAll();
        PricingPlanResponse? Create(PricingPlanRequest request);
        UpdatePricingResponse UpdatePricing(UpdatePricingRequest request);
        bool Delete(int id);
        ReportPricingStaffResponse GetReportPricingStaff(int userId);
        List<ListPricingResponse> GetListPricing(int userId);
        ReportRevenueResponse GetRevenueReport(int userId);
    }
}
