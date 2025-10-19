namespace BE_SWP391.Models.DTOs.Response
{
    public class DataForAdminResponse
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string ProviderName { get; set; }
        public string CategoryName { get; set; }

        public long? FileSize { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }

    }
}
