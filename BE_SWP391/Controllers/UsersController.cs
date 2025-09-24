using BE_SWP391.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly EvMarketContext _context;
        public UsersController(EvMarketContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            if (user == null)
            {
                return Ok(new ApiResponse {
                    Success = false,
                    Message = "Invalid Email/Password"
                }   );
            }
            return Ok(new ApiResponse
            {
                Success = true,
                message = "Login successful", 
                Data = token
            });
        }
    }
}
