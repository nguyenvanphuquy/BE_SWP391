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
                .FirstOrDefault(c => c.UserId == request.UserId &&
                                     c.PlanId == request.PlanId &&
                                     c.Status == "Pending");

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

        public List<CartResponse> GetList(int userId)
        {
            var query = from c in _marketContext.Carts
                        join pp in _marketContext.PricingPlans on c.PlanId equals pp.PlanId
                        join dp in _marketContext.DataPackages on pp.PackageId equals dp.PackageId
                        join mt in _marketContext.MetaDatas on dp.MetadataId equals mt.MetadataId
                        join u in _marketContext.Users on dp.UserId equals u.UserId
                        where c.UserId == userId && c.Status == "Pending"
                        select new
                        {
                            c.CartId,
                            pp.PlanId,
                            dp.PackageName,
                            c.Quantity,
                            ProviderName = u.FullName,
                            mt.Type,
                            mt.FileFormat,
                            TotalAmout = c.Quantity * pp.Price,
                        };

            var data = query.ToList();
            var totalPrice = data.Sum(x => x.TotalAmout);

            return data.Select(x => new CartResponse
            {
                CartId = x.CartId,
                PlanId = x.PlanId,
                PackageName = x.PackageName,
                Quantity = x.Quantity,
                ProviderName = x.ProviderName,
                Type = x.Type,
                FileFormat = x.FileFormat,
                TotalAmout = x.TotalAmout,
                TotalPrice = totalPrice
            }).ToList();
        }

        public Cart GetById(int cartId)
        {
            return _marketContext.Carts.FirstOrDefault(c => c.CartId == cartId);
        }

        public void Update(Cart cart)
        {
            _marketContext.Carts.Update(cart);
        }

        public void Delete(int cartId)
        {
            var cart = GetById(cartId);
            if (cart != null)
            {
                _marketContext.Carts.Remove(cart);
                _marketContext.SaveChanges();
            }
        }

        public bool UpdateQuantity(int cartId, int newQuantity)
        {
            var cart = _marketContext.Carts
                .FirstOrDefault(c => c.CartId == cartId && c.Status == "Pending");

            if (cart == null) return false;

            cart.Quantity = newQuantity;
            cart.UpdatedAt = DateTime.Now;

            _marketContext.Carts.Update(cart);
            _marketContext.SaveChanges();

            return true;
        }

        public void Save()
        {
            _marketContext.SaveChanges();
        }
    }
}
