namespace BE_SWP391.Models.DTOs.Common
{
    public class FileDownloadResult
    {
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
