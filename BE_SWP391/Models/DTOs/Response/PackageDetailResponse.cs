using BE_SWP391.Models.DTOs.Common;
using System.Diagnostics.Contracts;

namespace BE_SWP391.Models.DTOs.Response
{
    public class PackageDetailResponse
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string ProviderName { get; set; }
        public string SubCategoryName { get; set; }
        public long? FileSize { get; set; }
        public decimal Price { get; set; }
        public int? Duration { get; set; }
        public int FileCount { get; set; }
        public string Status { get; set; }
        public DateOnly? ReleaseDate { get; set; }

        public List<DownloadInfo>? Downloads { get; set; } = new List<DownloadInfo>();

    }
}
