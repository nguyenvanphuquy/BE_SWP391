using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models;
using BE_SWP391.Services.Interfaces;

namespace BE_SWP391.Repositories.Implementations
{
    public class DownloadService : IDownloadService
    {
        private readonly IDownloadRepository _downloadRepository;
        public DownloadService(IDownloadRepository downloadRepository)
        {
            _downloadRepository = downloadRepository;
        }
        public DownloadResponse? GetById(int id)
        {
            var download = _downloadRepository.GetById(id);
            return download == null ? null : ToResponse(download);
        }
        public IEnumerable<DownloadResponse> GetAll()
        {
            return _downloadRepository.GetAll().Select(d => ToResponse(d));
        }
        public DownloadResponse Creater(DownloadRequest request)
        {
            var download = new Download
            {
                TransactionId = request.TransactionId,
                PackageId = request.PackageId,
                DownloadDate = DateTime.UtcNow,
                FileUrl = request.FileUrl,
                FileHash = request.FileHash,
                Status = "Completed"
            };
            _downloadRepository.Create(download);
            return ToResponse(download);
        }
        public DownloadResponse? Update(int id, DownloadRequest request)
        {
            var download = _downloadRepository.GetById(id);
            if (download == null) return null;
            download.TransactionId = request.TransactionId;
            download.PackageId = request.PackageId;
            download.FileUrl = request.FileUrl;
            download.FileHash = request.FileHash;
            download.Status = request.Status;
            _downloadRepository.Update(download);
            return ToResponse(download);
        }
        public bool Delete(int id)
        {
            var download = _downloadRepository.GetById(id);
            if (download == null) return false;
            _downloadRepository.Delete(download);
            return true;
        }
        public static DownloadResponse ToResponse(Download download)
        {
            return new DownloadResponse
            {
                DownloadId = download.DownloadId,
                TransactionId = download.TransactionId,
                PackageId = download.PackageId,
                DownloadDate = download.DownloadDate,
                FileUrl = download.FileUrl,
                FileHash = download.FileHash,
                Status = download.Status

            };
        }
    }
}
