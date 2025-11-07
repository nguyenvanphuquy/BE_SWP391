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
        public DashboardController(IReportService reportService)
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
        [HttpGet("top-packages")]
        public IActionResult GetTopPackages(int top = 10)
        {
            return Ok(_reportService.GetTopPackages(top));
        }
        [HttpGet("top-providers")]
        public IActionResult GetTopProviders([FromQuery] int top = 10)
        {
            var result = _reportService.GetTopProviders(top);
            return Ok(result);
        }
        [HttpGet("category-analytics")]
        public IActionResult GetCategoryAnalytics()
        {
            var result = _reportService.GetCategoryAnalytics();
            return Ok(result);
        }
        [HttpGet("dashboard/summary")]
        public IActionResult GetSummary()
        {
            var result = _reportService.GetDashboardSummary();
            return Ok(result);
        }
        [HttpGet("order-report/{userId}")]
        public IActionResult GetOrderReport(int userId)
        {
            var result = _reportService.GetOrderReport(userId);
            return Ok(result);
        }
        [HttpGet("order-list/{userId}")]
        public IActionResult GetOrderList(int userId)
        {
            var result = _reportService.GetOrderList(userId);
            return Ok(result);
        }
        [HttpGet("order-detail/{invoiceId}")]
        public IActionResult GetOrderDetail(int invoiceId)
        {
            var result = _reportService.GetOrderDetail(invoiceId);
            return Ok(result);
        }
    }
}
