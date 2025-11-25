namespace BE_SWP391.Models.DTOs.Response
{
    public class DownloadResponse
    {
        public int DownloadId { get; set; }
        public int PackageId { get; set; }
        public DateTime? DownloadDate { get; set; }
        public string FileUrl { get; set; }
        public string FileHash { get; set; }
        public string Status { get; set; }
        public int? DownloadCount { get; set; }
    }
}
