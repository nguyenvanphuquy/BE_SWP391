using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueShareController : ControllerBase
    {
        private readonly IRevenueShareService _revenueShareService;
        public RevenueShareController(IRevenueShareService revenueShareService)
        {
            _revenueShareService = revenueShareService;
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var revenueShare = _revenueShareService.GetById(id);
            if (revenueShare == null)
            {
                return NotFound();
            }
            return Ok(revenueShare);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var revenueShares = _revenueShareService.GetAll();
            return Ok(revenueShares);
        }
        [HttpPost]
        public IActionResult Create(RevenueShareRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var revenueShare = _revenueShareService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = revenueShare.RevenueId }, revenueShare);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, RevenueShareRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var revenueShare = _revenueShareService.Update(id, request);
            if (revenueShare == null)
            {
                return NotFound();
            }
            return Ok(revenueShare);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _revenueShareService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("Profit")]
        public IActionResult GetProfit()
        {
            return Ok(_revenueShareService.GetAllProfit());
        }
    }
}
