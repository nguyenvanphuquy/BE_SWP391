using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Common;
using Microsoft.AspNetCore.Hosting;

namespace BE_SWP391.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _uploadPath;

        public FileService(IWebHostEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            // Sử dụng WebRootPath (wwwroot)
            var webRootPath = environment.WebRootPath;

            if (string.IsNullOrEmpty(webRootPath))
            {
                // Fallback nếu WebRootPath null
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            _uploadPath = Path.Combine(webRootPath, "uploads", "downloads");

            Console.WriteLine($"📁 Upload path: {_uploadPath}");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
                Console.WriteLine($"✅ Created directory: {_uploadPath}");
            }
        }

        public FileUploadResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            try
            {
                Console.WriteLine($"📁 Starting file upload: {file.FileName}");

                // Tạo tên file unique
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(_uploadPath, fileName);

                Console.WriteLine($"💾 Saving file to: {filePath}");

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                Console.WriteLine($"✅ File saved successfully");

                // Tính hash SHA256
                var fileHash = CalculateSHA256Hash(filePath);
                Console.WriteLine($"🔐 File hash: {fileHash}");

                return new FileUploadResult
                {
                    FileUrl = $"/uploads/downloads/{fileName}", // URL để truy cập file
                    FileName = file.FileName,
                    FileHash = fileHash,
                    FileSize = file.Length
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error uploading file: {ex.Message}");
                throw;
            }
        }

        public FileDownloadResult DownloadFile(string fileUrl)
        {
            try
            {
                var fileName = Path.GetFileName(fileUrl);
                var filePath = Path.Combine(_uploadPath, fileName);

                Console.WriteLine($"📥 Downloading file from: {filePath}");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException("File không tồn tại");

                var fileContent = File.ReadAllBytes(filePath);
                var contentType = GetContentType(filePath);

                Console.WriteLine($"✅ File downloaded: {fileName}");

                return new FileDownloadResult
                {
                    FileContent = fileContent,
                    ContentType = contentType,
                    FileName = Path.GetFileName(filePath)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error downloading file: {ex.Message}");
                throw;
            }
        }

        public bool DeleteFile(string fileUrl)
        {
            try
            {
                var fileName = Path.GetFileName(fileUrl);
                var filePath = Path.Combine(_uploadPath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"✅ File deleted: {filePath}");
                    return true;
                }

                Console.WriteLine($"⚠️ File not found for deletion: {filePath}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error deleting file: {ex.Message}");
                return false;
            }
        }

        private string CalculateSHA256Hash(string filePath)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".zip", "application/zip" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".ppt", "application/vnd.ms-powerpoint" },
                { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".txt", "text/plain" },
                { ".csv", "text/csv" },
                { ".json", "application/json" },
                { ".xml", "application/xml" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" }
            };

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
    }
}