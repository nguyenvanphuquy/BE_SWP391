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
        [HttpGet("{id}")]
        public ActionResult<DownloadResponse> GetById(int id)
        {
            var download = _downloadService.GetById(id);
            if (download == null) return NotFound();
            return Ok(download);
        }
        [HttpGet]
        public ActionResult<IEnumerable<DownloadResponse>> GetAll()
        {

            var downloads = _downloadService.GetAll();
            return Ok(downloads);
        }
        [HttpPost]
        public ActionResult<DownloadResponse> Create([FromBody] DownloadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var download = _downloadService.Creater(request);
            return CreatedAtAction(nameof(GetById), new { id = download.DownloadId }, download);
        }
        [HttpPut("{id}")]
        public ActionResult<DownloadResponse> Update(int id, [FromBody] DownloadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var download = _downloadService.Update(id, request);
            if (download == null) return NotFound();
            return Ok(download);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = _downloadService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
