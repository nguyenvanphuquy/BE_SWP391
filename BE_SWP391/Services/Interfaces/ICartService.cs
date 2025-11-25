using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;

namespace BE_SWP391.Services.Interfaces
{
    public interface ICartService
    {
        AddToCartResponse AddToCart(AddToCartRequest request);
        List<CartResponse> GetList(int userId);
        bool UpdateQuantity(int cartId, int quantity);
        bool Delete(int cartId);
    }
}
