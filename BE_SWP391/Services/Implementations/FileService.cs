using BE_SWP391.Models.DTOs.Common;
using BE_SWP391.Services.Interfaces;

namespace BE_SWP391.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _uploadPath;

        public FileService(IWebHostEnvironment environment)
        {
            _uploadPath = Path.Combine(environment.WebRootPath, "uploads", "downloads");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        public FileUploadResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileHash = CalculateFileHash(filePath);

            return new FileUploadResult
            {
                FileUrl = $"/uploads/downloads/{fileName}",
                FileName = file.FileName,
                FileHash = fileHash,
                FileSize = file.Length
            };
        }

        public FileDownloadResult DownloadFile(string fileUrl)
        {
            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(_uploadPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File không tồn tại");

            var fileContent = File.ReadAllBytes(filePath);
            var contentType = GetContentType(filePath);

            return new FileDownloadResult
            {
                FileContent = fileContent,
                ContentType = contentType,
                FileName = Path.GetFileName(filePath)
            };
        }

        public bool DeleteFile(string fileUrl)
        {
            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(_uploadPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }

        private string CalculateFileHash(string filePath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".zip", "application/zip" },
                { ".rar", "application/x-rar-compressed" },
                { ".txt", "text/plain" },
                { ".csv", "text/csv" },
                { ".json", "application/json" },
                { ".xml", "application/xml" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
            };

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
    }
}