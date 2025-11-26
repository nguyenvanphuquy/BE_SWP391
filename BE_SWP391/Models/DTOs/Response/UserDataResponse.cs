using System.Runtime.CompilerServices;

namespace BE_SWP391.Models.DTOs.Response
{
    public class UserDataResponse
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public long? FileSize { get; set; }
        public string SubCategoryName { get; set; }
        public string status { get; set; }
        public decimal DownloadCount { get; set; }
        public decimal? Price { get; set; }
    }
}
