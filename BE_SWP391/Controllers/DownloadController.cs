using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IDownloadService _downloadService;
        public DownloadController(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var downloads = _downloadService.GetAll();
            return Ok(downloads);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var download = _downloadService.GetById(id);
            if (download == null)
                return NotFound();
            return Ok(download);
        }

        [HttpGet("package/{packageId}")]
        public IActionResult GetByPackageId(int packageId)
        {
            var downloads = _downloadService.GetDownloadsByPackageId(packageId);
            return Ok(downloads);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DownloadRequest request)
        {
            var result = _downloadService.Create(request);
            return Ok(result);
        }

        [HttpPost("with-file")]
        public IActionResult CreateWithFile([FromForm] CreateDownloadWithFileRequest request)
        {
            try
            {
                // Validate
                if (request.File == null)
                    return BadRequest(new { message = "File là bắt buộc" });

                if (request.PackageId <= 0)
                    return BadRequest(new { message = "PackageId phải lớn hơn 0" });

                var result = _downloadService.CreateWithFileUpload(request);
                return Ok(new
                {
                    message = "Tạo download thành công",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DownloadRequest request)
        {
            var result = _downloadService.Update(id, request);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}/with-file")]
        public IActionResult UpdateWithFile(int id, [FromForm] UpdateDownloadWithFileRequest request)
        {
            try
            {
                var result = _downloadService.UpdateWithFile(id, request);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _downloadService.Delete(id);
            if (!result)
                return NotFound();
            return Ok(new { message = "Xóa download thành công" });
        }

        [HttpGet("{id}/download")]
        public IActionResult DownloadFile(int id)
        {
            try
            {
                var result = _downloadService.DownloadFile(id);
                return File(result.FileContent, result.ContentType, result.FileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}