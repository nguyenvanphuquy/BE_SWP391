using BE_SWP391.Models.DTOs.Common;

namespace BE_SWP391.Models.DTOs.Response
{
    public class DataForUserResponse
    {
        public int TransactionId { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string ProviderName { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string FileFormat { get; set; }
        public long? FileSize { get; set; }
        public int TotalDownloads { get; set; }
        public DateTime? LatestDownloadDate { get; set; }
        public string Status { get; set; }
        public List<DownloadInfo>? Downloads { get; set; } = new List<DownloadInfo>();
    }
}