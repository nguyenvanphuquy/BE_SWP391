namespace BE_SWP391.Models.DTOs.Request
{
    public class PricingPlanRequest
    {
        public string PlanName { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Currency { get; set; }

        public int? Duration { get; set; }

        public string? AccessType { get; set; }

        public string PackageName { get; set; } 

        public decimal? Discount { get; set; }
    }
}
