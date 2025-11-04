using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataPackageController : ControllerBase
    {
        private readonly IDataPackageService _dataPackageService;
        public DataPackageController(IDataPackageService dataPackageService)
        {
            _dataPackageService = dataPackageService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dataPackages = _dataPackageService.GetAll();
            return Ok(dataPackages);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dataPackage = _dataPackageService.GetById(id);
            if (dataPackage == null)
            {
                return NotFound();
            }
            return Ok(dataPackage);
        }
        [HttpGet("Count")]
        public IActionResult GetStatusCount()
        {
            var count = _dataPackageService.GetStatusCount();
            return Ok(count);
        }
        [HttpGet("DataForAdmin")]
        public IActionResult GetDataForAdmin()
        {
            var data = _dataPackageService.GetDataForAdmin();
            return Ok(new
            {


                success = true,
                data = data
            });
        }
        [HttpPost]
        public IActionResult Create([FromBody] DataPackageRequest request)
        {

            try
            {
                _dataPackageService.Create(request);
                return Ok(new { message = "Đăng ký bộ dữ liệu thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DataPackageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var dataPackage = _dataPackageService.Update(id, request);
            if (dataPackage == null)
            {
                return NotFound();
            }
            return Ok(dataPackage);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _dataPackageService.Detele(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("{packageId}/status")]
        public IActionResult ChageStatus(int packageId, ChageStatusRequest request)
        {
            var ChageStatus = _dataPackageService.ChageStatus(packageId, request);
            if (!ChageStatus)
            {
                return NotFound(new { message = "Không tìm thấy gói dữ liệu" });
            }

            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }
        [HttpGet("pending")]
        public IActionResult GetPendingPackages()
        {
            var data = _dataPackageService.GetPendingData();
            return Ok(data);
        }
        [HttpGet("AllPackage")]
        public IActionResult GetAllPackage()
        {
            return Ok(_dataPackageService.GetAllPackages());
        }
        [HttpGet("dashboard/{userId}")]
        public IActionResult GetDashboardStatsByUser(int userId)
        {
            var result = _dataPackageService.GetUserDataStats(userId);
            return Ok(result);
        }
        [HttpGet("user/{userId}")]
        public ActionResult<List<UserDataResponse>> GetUserData(int userId)
        {
            var result = _dataPackageService.GetUserDataByUserId(userId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy dữ liệu cho user này." });
            }

            return Ok(result);
        }
        [HttpGet("user-data/{userId}")]
        public IActionResult GetDataForUser(int userId)
        {
            var data = _dataPackageService.GetDataForUser(userId);
            if (data == null || data.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy dữ liệu cho user này." });
            }else
                return Ok(data);
        }
    }
}
