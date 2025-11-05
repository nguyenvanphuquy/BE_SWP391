namespace BE_SWP391.Models.DTOs.Response
{
    public class ReportOrderResponse
    {
        public int TotalPackage { get; set; }
        public int NewPackageThisMonth { get; set; }
        public int TotalDownload { get; set; }
        public int TotalRemaining { get; set; }
        public int ActiveCount { get; set; }
        public int ExpiredCount { get; set; }
    }
}
