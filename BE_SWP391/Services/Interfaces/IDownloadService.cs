using BE_SWP391.Models.DTOs.Common;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface IDownloadService
    {
        DownloadResponse GetById(int id);
        IEnumerable<DownloadResponse> GetAll();
        IEnumerable<DownloadResponse> GetDownloadsByPackageId(int packageId);
        DownloadResponse Create(DownloadRequest request);
        DownloadResponse CreateWithFileUpload(CreateDownloadWithFileRequest request);
        DownloadResponse Update(int id, DownloadRequest request);
        DownloadResponse UpdateWithFile(int id, UpdateDownloadWithFileRequest request);
        bool Delete(int id);
        FileDownloadResult DownloadFile(int downloadId, int roleId);

    }
}
