using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Common;
using BE_SWP391.Services.Interfaces;

namespace BE_SWP391.Services.Implementations
{
    public class DownloadService : IDownloadService
    {
        private readonly IDownloadRepository _downloadRepository;
        private readonly IDataPackageRepository _packageRepository;
        private readonly IFileService _fileService;

        public DownloadService(
            IDownloadRepository downloadRepository,
            IDataPackageRepository packageRepository,
            IFileService fileService)
        {
            _downloadRepository = downloadRepository;
            _packageRepository = packageRepository;
            _fileService = fileService;
        }

        public DownloadResponse GetById(int id)
        {
            var download = _downloadRepository.GetById(id);
            return download == null ? null : ToResponse(download);
        }

        public IEnumerable<DownloadResponse> GetAll()
        {
            return _downloadRepository.GetAll().Select(d => ToResponse(d));
        }

        public IEnumerable<DownloadResponse> GetDownloadsByPackageId(int packageId)
        {
            return _downloadRepository.GetByPackageId(packageId).Select(d => ToResponse(d));
        }

        public DownloadResponse Create(DownloadRequest request)
        {
            var download = new Download
            {
                PackageId = request.PackageId,
                DownloadDate = DateTime.UtcNow,
                FileUrl = request.FileUrl,
                FileHash = request.FileHash,
                Status = request.Status ?? "Active",
                DownloadCount = 0
            };
            _downloadRepository.Create(download);
            return ToResponse(download);
        }

        public DownloadResponse CreateWithFileUpload(CreateDownloadWithFileRequest request)
        {
            // Kiểm tra package tồn tại
            var package = _packageRepository.GetById(request.PackageId);
            if (package == null)
                throw new ArgumentException($"Package với ID {request.PackageId} không tồn tại");

            // Upload file
            var fileUploadResult = _fileService.UploadFile(request.File);

            // Tạo download record
            var download = new Download
            {
                PackageId = request.PackageId,
                DownloadDate = DateTime.UtcNow,
                FileUrl = fileUploadResult.FileUrl,
                FileHash = fileUploadResult.FileHash,
                Status = "Active",
                DownloadCount = 0
            };

            _downloadRepository.Create(download);
            return ToResponse(download);
        }

        public DownloadResponse Update(int id, DownloadRequest request)
        {
            var download = _downloadRepository.GetById(id);
            if (download == null) return null;

            download.PackageId = request.PackageId;
            download.FileUrl = request.FileUrl;
            download.FileHash = request.FileHash;
            download.Status = request.Status;

            _downloadRepository.Update(download);
            return ToResponse(download);
        }

        public DownloadResponse UpdateWithFile(int id, UpdateDownloadWithFileRequest request)
        {
            var download = _downloadRepository.GetById(id);
            if (download == null) return null;

            // Nếu có file mới, upload file
            if (request.File != null)
            {
                var fileUploadResult = _fileService.UploadFile(request.File);
                download.FileUrl = fileUploadResult.FileUrl;
                download.FileHash = fileUploadResult.FileHash;
            }

            download.PackageId = request.PackageId;
            download.Status = request.Status;

            _downloadRepository.Update(download);
            return ToResponse(download);
        }

        public bool Delete(int id)
        {
            var download = _downloadRepository.GetById(id);
            if (download == null) return false;

            // Xóa file vật lý nếu cần
            if (!string.IsNullOrEmpty(download.FileUrl) && download.FileUrl.StartsWith("/uploads/"))
            {
                _fileService.DeleteFile(download.FileUrl);
            }

            _downloadRepository.Delete(download);
            return true;
        }

        public FileDownloadResult DownloadFile(int downloadId)
        {
            var download = _downloadRepository.GetById(downloadId);
            if (download == null)
                throw new FileNotFoundException("Download không tồn tại");

            if (string.IsNullOrEmpty(download.FileUrl))
                throw new FileNotFoundException("File không tồn tại");

            // Tăng download count
            download.DownloadCount++;
            _downloadRepository.Update(download);

            // Download file từ server
            return _fileService.DownloadFile(download.FileUrl);
        }

        public static DownloadResponse ToResponse(Download download)
        {
            return new DownloadResponse
            {
                DownloadId = download.DownloadId,
                PackageId = download.PackageId,
                DownloadDate = download.DownloadDate,
                FileUrl = download.FileUrl,
                FileHash = download.FileHash,
                Status = download.Status,
                DownloadCount = download.DownloadCount
            };
        }
    }
}