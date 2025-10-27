using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IReportService _reportService;
        public DashboardController( IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        public IActionResult GetDashboard()
        {
            return Ok(_reportService.DashboardStats());
        }
        [HttpGet("Total")]
        public IActionResult GetTotal()
        {
            return Ok(_reportService.GetTotal());
        }
    }
}
