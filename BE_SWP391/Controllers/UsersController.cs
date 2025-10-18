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
        [HttpGet("Infor")]
        public IActionResult GetUserInfor()
        {
            var users = _userService.GetAllInfor();
            return Ok(users);
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var user = _userService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                });
            }


        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var user = _userService.Register(request);
                return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
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
            {
                return BadRequest(ModelState);
            }
            var response = _userService.Login(request);
            if (response == null) return Unauthorized(new ApiResponse
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
        [HttpPost("logout")]
        public IActionResult Logout()
            {

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Logout successful"
                });
            }
        
    }
}