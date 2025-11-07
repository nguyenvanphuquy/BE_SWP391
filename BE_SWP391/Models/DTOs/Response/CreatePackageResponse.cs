namespace BE_SWP391.Models.DTOs.Response
{
    public class CreatePackageResponse
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string SubCategoryName { get; set; }
        public int UserId { get; set; }

        public string Type { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string Keywords { get; set; }
        public string FileFormat { get; set; }
        public long? FileSize { get; set; }

        public DateOnly? ReleaseDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Status { get; set; }

        public string Message { get; set; }
    }
}
