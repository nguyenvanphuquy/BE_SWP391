namespace BE_SWP391.Models.DTOs.Response
{
    public class TopPackageResponse
    {
            
        public string PackageName { get; set; }
        public int TotalDownloads { get; set; }
        public string ProviderName { get; set; }
        public string CategoryName { get; set; }
        public double GrowthRatePercent { get; set; }
    }
}

