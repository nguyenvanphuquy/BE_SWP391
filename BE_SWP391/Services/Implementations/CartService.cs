using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
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
    }
}
