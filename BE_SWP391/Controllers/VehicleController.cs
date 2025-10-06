using BE_SWP391.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var vehicles = _vehicleService.GetAll();
            ;
            return Ok(vehicles);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var vehicle = _vehicleService.GetById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        [HttpPost]
        public IActionResult Create([FromBody] VehicleRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vehicle = _vehicleService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.VehicleId}, vehicle);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] VehicleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vehicle = _vehicleService.Update(id, request);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _vehicleService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
