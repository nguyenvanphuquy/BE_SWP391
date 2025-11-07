namespace BE_SWP391.Models.DTOs.Request
{
    public class DataPackageRequest
    {
        public string PackageName { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public int UserId { get; set; }
        public string SubCategoryName { get; set; }
        public MetaDataRequest MetaData { get; set; }

    }
}
