namespace BE_SWP391.Models.DTOs.Response
{
    public class PricingPlanResponse
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Currency { get; set; }

        public int? Duration { get; set; }

        public string? AccessType { get; set; }

        public int PackageId { get; set; }

        public decimal? Discount { get; set; }
    }
}
