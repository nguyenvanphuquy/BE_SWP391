namespace BE_SWP391.Models.DTOs.Common
{
    public class DownloadInfo
    {
        public int DownloadId { get; set; }
        public DateTime? DownloadDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Status { get; set; }
        public int? DownloadCount { get; set; }
        public int PackageId { get; set; } 
    }
}