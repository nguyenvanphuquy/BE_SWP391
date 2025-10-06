using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface IPaymentMethodRepository
    {
        PaymentMethod? GetById(int id);
        IEnumerable<PaymentMethod> GetAll();
        void Create(PaymentMethod paymentMethod);
        void Update(PaymentMethod paymentMethod);
        void Delete(PaymentMethod paymentMethod);

    }
}
