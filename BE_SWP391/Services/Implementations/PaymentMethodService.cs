using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.Entities;
using System.Net.WebSockets;
namespace BE_SWP391.Services.Implementations
{
    public class PaymentMethodService: IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }
        public PaymentMethodResponse? GetById(int id)
        {
            var paymentMethod = _paymentMethodRepository.GetById(id);
            return paymentMethod == null ? null : ToResponse(paymentMethod);
        }
        public IEnumerable<PaymentMethodResponse> GetAll()
        {
            return _paymentMethodRepository.GetAll().Select(pm => ToResponse(pm));
        }
        public PaymentMethodResponse? Create(PaymentMethodRequest request)
        {
            var paymentMethod = new PaymentMethod
            {
                MethodName = request.MethodName,
                Provider = request.Provider,
                Details = request.Details,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                TransactionId = request.TransactionId
            };
            _paymentMethodRepository.Create(paymentMethod);
            return ToResponse(paymentMethod);
        }
        public PaymentMethodResponse? Update(int id, PaymentMethodRequest request)
        {
            var paymentMethod = _paymentMethodRepository.GetById(id);
            if (paymentMethod == null)
            {
                return null;
            }
            paymentMethod.MethodName = request.MethodName;
            paymentMethod.Provider = request.Provider;
            paymentMethod.Details = request.Details;
            paymentMethod.Status = request.Status;
            paymentMethod.TransactionId = request.TransactionId;
            if (request.CreatedAt.HasValue)
            {
                paymentMethod.CreatedAt = request.CreatedAt.Value;
            }

            _paymentMethodRepository.Update(paymentMethod);
            return ToResponse(paymentMethod);
        }
        public bool Delete(int id)
        {
            var paymentMethod = _paymentMethodRepository.GetById(id);
            if (paymentMethod == null)
            {
                return false;
            }
            _paymentMethodRepository.Delete(paymentMethod);
            return true;
        }
        public static PaymentMethodResponse ToResponse(PaymentMethod paymentMethod)
        {
            return new PaymentMethodResponse
            {
                MethodId = paymentMethod.MethodId,
                MethodName = paymentMethod.MethodName,
                Provider = paymentMethod.Provider,
                Details = paymentMethod.Details,
                Status = paymentMethod.Status,
                CreatedAt = paymentMethod.CreatedAt,
                TransactionId = paymentMethod.TransactionId
            };
        }

    }
}
