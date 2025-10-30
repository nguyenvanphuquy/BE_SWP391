namespace BE_SWP391.Models.DTOs.Response
{
    public class AllPackageResponse
    {
        public int PrincingPlanId { get; set; }
        public string PackageName { get; set; }
        public string ProviderName { get; set; }
        public string Type { get; set; }
        public Double Rating { get; set; }
        public int DownloadCount { get; set; }
        public int CartCount { get; set; }
        public DateOnly? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal PricingPlan { get; set; }
        public string Description {  get; set; }
        public string? FileFormat { get; set; }
        public string? Version { get; set; }
        public long? FileSize { get; set; }

    }
}
