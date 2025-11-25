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
        // Update quantity
        [HttpPut("update-quantity")]
        public IActionResult UpdateQuantity([FromBody] UpdateCartQuantityRequest request)
        {
            var result = _cartService.UpdateQuantity(request.CartId, request.Quantity);

            if (!result)
                return NotFound(new { message = "Không tìm thấy cart item!" });

            return Ok(new { message = "Cập nhật số lượng thành công!" });
        }

        // Delete item
        [HttpDelete("delete/{cartId}")]
        public IActionResult Delete(int cartId)
        {
            var result = _cartService.Delete(cartId);

            if (!result)
                return NotFound(new { message = "Không tìm thấy cart item!" });

            return Ok(new { message = "Xóa cart item thành công!" });
        }
    }
}

