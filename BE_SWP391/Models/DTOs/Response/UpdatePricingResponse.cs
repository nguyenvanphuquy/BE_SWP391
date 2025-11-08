namespace BE_SWP391.Models.DTOs.Response
{
    public class UpdatePricingResponse
    {
        public int PricingPlanId { get; set; }
        public string PacageName { get; set; }
        public string Description { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
    }
}
