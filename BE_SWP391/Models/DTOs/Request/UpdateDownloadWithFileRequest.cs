namespace BE_SWP391.Models.DTOs.Request
{
    public class UpdateDownloadWithFileRequest
    {
        public int PackageId { get; set; }
        public string Status { get; set; }
        public IFormFile File { get; set; }
    }
}
