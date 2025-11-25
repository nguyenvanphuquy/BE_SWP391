using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Implementations;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;

namespace BE_SWP391.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }
        public AddToCartResponse AddToCart(AddToCartRequest request)
        {
            return _repository.AddToCart(request);
        }
        public List<CartResponse> GetList(int userId)
        {
            return _repository.GetList(userId);
        }
        public bool UpdateQuantity(int cartId, int quantity)
        {
            return _repository.UpdateQuantity(cartId, quantity);
        }

        public bool Delete(int cartId)
        {
            var cart = _repository.GetById(cartId);
            if (cart == null) return false;

            _repository.Delete(cartId);
            return true;
        }
    }
}
