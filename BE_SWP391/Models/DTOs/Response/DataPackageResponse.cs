namespace BE_SWP391.Models.DTOs.Response
{
    public class DataPackageResponse
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Version { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string? Status { get; set; }
        public int UserId { get; set; }
        public int? SubcategoryId { get; set; }
        public int? MetadataId { get; set; }
    }
}
