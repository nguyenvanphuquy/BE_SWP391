namespace BE_SWP391.Models.DTOs.Common
{
    public class FileUploadResult
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileHash { get; set; }
        public long FileSize { get; set; }
    }
}
