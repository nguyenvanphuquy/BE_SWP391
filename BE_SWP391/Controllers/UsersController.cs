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
        {
        }
        [HttpPost("login")]
        {
            {
                return Ok(new ApiResponse {
                    Success = false,
            return Ok(new ApiResponse
            {
                Success = true,
            });
        }


    }
}
