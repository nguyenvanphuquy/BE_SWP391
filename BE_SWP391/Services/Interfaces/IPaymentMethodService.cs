using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        PaymentMethodResponse? GetById(int id);
        IEnumerable<PaymentMethodResponse> GetAll();
        PaymentMethodResponse? Create(PaymentMethodRequest request);
        PaymentMethodResponse? Update(int id, PaymentMethodRequest request);
        bool Delete(int id);
    }
}
