using BE_SWP391.Models.DTOs.Common;

namespace BE_SWP391.Services.Interfaces
{
    public interface IFileService
    {
        FileUploadResult UploadFile(IFormFile file);
        FileDownloadResult DownloadFile(string fileUrl);
        bool DeleteFile(string fileUrl);
    }
}
