using BE_SWP391.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.Entities;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _userService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _userService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _userService.Login(request);
            if (response == null)
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                });

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Login successful",
                Data = response
            });
        }
    }
}