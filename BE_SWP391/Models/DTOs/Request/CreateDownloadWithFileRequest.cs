namespace BE_SWP391.Models.DTOs.Request
{
    public class CreateDownloadWithFileRequest
    {
        public int PackageId { get; set; }
        public IFormFile File { get; set; }
    }
}
