namespace BE_SWP391.Models.DTOs.Response
{
    public class AllPackageResponse
    {
        public string PackageName { get; set; }
        public string ProviderName { get; set; }
        public string Type { get; set; }
        public Double Rating { get; set; }
        public int DownloadCount { get; set; }
        public DateOnly? CreateAt { get; set; }
        public decimal PricingPlan { get; set; }
    }
}
