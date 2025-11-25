using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface ICartRepository
    {
        AddToCartResponse AddToCart(AddToCartRequest request);
        List<CartResponse> GetList(int userId);

        Cart GetById(int cartId);
        void Update(Cart cart);
        void Delete(int cartId);
        void Save();

        // NEW
        bool UpdateQuantity(int cartId, int newQuantity);
    }
}
