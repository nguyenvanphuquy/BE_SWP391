using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface ICartService
    {
        AddToCartResponse AddToCart(AddToCartRequest request);
    }
}
