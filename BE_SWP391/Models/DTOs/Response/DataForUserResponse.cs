namespace BE_SWP391.Models.DTOs.Response
{
    public class DataForUserResponse
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string ProviderName { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string FileFormat { get; set; }
        public long? FileSize { get; set; }
        public string Status { get; set; }
    }
}
