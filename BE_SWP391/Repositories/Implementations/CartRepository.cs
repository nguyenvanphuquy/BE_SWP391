using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_SWP391.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly EvMarketContext _marketContext;
        public CartRepository(EvMarketContext marketContext)
        {
            _marketContext = marketContext;
        }
        public AddToCartResponse AddToCart(AddToCartRequest request)
        {
            var existingItem = _marketContext.Carts
                .FirstOrDefault(c => c.UserId == request.UserId && c.PlanId == request.PlanId && c.Status == "Pending");

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.UpdatedAt = DateTime.Now;
            }
            else
            {
                var cart = new Cart
                {
                    UserId = request.UserId,
                    PlanId = request.PlanId,
                    Quantity = request.Quantity,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _marketContext.Carts.Add(cart);
            }

            _marketContext.SaveChanges();

            int? totalItems = _marketContext.Carts
                .Where(c => c.UserId == request.UserId && c.Status == "Pending")
                .Sum(c => c.Quantity);

            return new AddToCartResponse
            {
                UserId = request.UserId,
                TotalItems = totalItems,
                Message = "Thêm vào giỏ hàng thành công!"
            };
        }
    }
}