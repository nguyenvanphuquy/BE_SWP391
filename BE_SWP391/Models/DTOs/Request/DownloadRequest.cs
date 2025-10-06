namespace BE_SWP391.Models.DTOs.Request
{
    public class DownloadRequest
    {
        public int TransactionId { get; set; }

        public int PackageId { get; set; }

        public DateTime? DownloadDate { get; set; }

        public string? FileUrl { get; set; }

        public string? FileHash { get; set; }

        public string? Status { get; set; }
    }
}
