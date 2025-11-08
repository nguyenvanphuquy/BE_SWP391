using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingPlanController : ControllerBase
    {
        private readonly IPricingPlanService _pricingPlanService;
        public PricingPlanController(IPricingPlanService pricingPlanService)
        {
            _pricingPlanService = pricingPlanService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var pricingPlans = _pricingPlanService.GetAll();
            return Ok(pricingPlans);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var pricingPlan = _pricingPlanService.GetById(id);
            if (pricingPlan == null) return NotFound();
            return Ok(pricingPlan);
        }
        [HttpPost]
        public IActionResult Create([FromBody] PricingPlanRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pricingPlan = _pricingPlanService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = pricingPlan.PackageId }, pricingPlan);
        }
        [HttpPut]
        public IActionResult UpdatePrice([FromBody] UpdatePricingRequest request)
        {
            try
            {
                var result = _pricingPlanService.UpdatePricing(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _pricingPlanService.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpGet("ReportPricingStaff/{userId}")]
        public IActionResult GetReportPricingStaff(int userId)
        {
            var report = _pricingPlanService.GetReportPricingStaff(userId);
            return Ok(report);
        }
        [HttpGet("ListPricing/{userId}")]
        public IActionResult GetListPricing(int userId)
        {
            var listPricing = _pricingPlanService.GetListPricing(userId);
            return Ok(listPricing);
        }
    }
}
