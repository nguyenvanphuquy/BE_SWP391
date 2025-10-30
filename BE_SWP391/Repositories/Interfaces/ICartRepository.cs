using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface ICartRepository
    {
        AddToCartResponse AddToCart(AddToCartRequest request);
    }
}
