using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;

namespace BE_SWP391.Services.Interfaces
{
    public interface IDownloadService
    {
        DownloadResponse? GetById(int id);
        IEnumerable<DownloadResponse> GetAll();
        DownloadResponse Creater(DownloadRequest request);
        DownloadResponse? Update(int id, DownloadRequest request);
        bool Delete(int id);

    }
}
