namespace BE_SWP391.Models.DTOs.Request
{
    public class UpdatePricingRequest
    {
        public int PricingPlanId { get; set; }
        public int NewPrice { get; set; }
    }
}
