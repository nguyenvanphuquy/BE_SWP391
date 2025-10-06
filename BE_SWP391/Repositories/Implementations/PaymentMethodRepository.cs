using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models;
using BE_SWP391.Repositories.Interfaces;
namespace BE_SWP391.Repositories.Implementations
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly EvMarketContext _context;
        public PaymentMethodRepository(EvMarketContext context)
        {
            _context = context;
        }
        public PaymentMethod? GetById(int id)
        {
            return _context.PaymentMethods.Find(id);
        }
        public IEnumerable<PaymentMethod> GetAll()
        {
            return _context.PaymentMethods.ToList();
        }
        public void Create(PaymentMethod paymentMethod)
        {
            _context.PaymentMethods.Add(paymentMethod);
            _context.SaveChanges();
        }
        public void Update(PaymentMethod paymentMethod)
        {
            _context.PaymentMethods.Update(paymentMethod);
            _context.SaveChanges();
        }
        public void Delete(PaymentMethod paymentMethod)
        {
                _context.PaymentMethods.Remove(paymentMethod);
                _context.SaveChanges();
        }

    }
}
