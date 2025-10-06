using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService regionService;
        public RegionController(IRegionService regionService)
        {
            this.regionService = regionService;
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var region = regionService.GetById(id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = regionService.GetAll();
            return Ok(regions);
        }
        [HttpPost]
        public IActionResult Create([FromBody] RegionRequest request)
        {
            var region = regionService.Create(request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(GetById), new { id = region.RegionId }, region);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] RegionRequest request)
        {
            var region = regionService.Update(id, request);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = regionService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
