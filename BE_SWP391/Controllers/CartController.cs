using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {
            return Ok(_cartService.AddToCart(request));
        }
        [HttpGet]
        public IActionResult GetById(int userId)
        {
            return Ok(_cartService.GetList(userId));
        }
    }
}
