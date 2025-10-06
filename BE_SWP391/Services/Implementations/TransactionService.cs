using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Services.Interfaces;
namespace BE_SWP391.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public TransactionResponse? GetById(int id)
        {
            var transaction = _transactionRepository.GetById(id);
            return transaction == null ? null : ToResponse(transaction);
        }
        public IEnumerable<TransactionResponse> GetAll()
        {
            return _transactionRepository.GetAll().Select(ToResponse);
        }
        public TransactionResponse? Create(TransactionRequest request)
        {
            var transaction = new Transaction
            {
                TransactionDate = request.TransactionDate,
                Status = request.Status,
                Amount = request.Amount,
                Currency = request.Currency,
                InvoiceId = request.InvoiceId
            };
            _transactionRepository.Create(transaction);
            return ToResponse(transaction);
        }
        public TransactionResponse? Update(int id, TransactionRequest request)
        {
            var transaction = _transactionRepository.GetById(id);
            if (transaction == null)
            {
                return null;
            }
            if (request.TransactionDate != null)
            {
                transaction.TransactionDate = request.TransactionDate;
            }
            if (!string.IsNullOrEmpty(request.Status))
            {
                transaction.Status = request.Status;
            }
            if (request.Amount != null)
            {
                transaction.Amount = request.Amount;
            }
            if (!string.IsNullOrEmpty(request.Currency))
            {
                transaction.Currency = request.Currency;
            }
            if (request.InvoiceId != null)
            {
                transaction.InvoiceId = request.InvoiceId;
            }
            _transactionRepository.Update(transaction);
            return ToResponse(transaction);
        }
        public bool Delete(int id)
        {
            var transaction = _transactionRepository.GetById(id);
            if (transaction == null)
            {
                return false;
            }
            _transactionRepository.Delete(transaction);
            return true;
        }
        public static TransactionResponse ToResponse(Transaction transaction)
        {
            return new TransactionResponse
            {
                TransactionId = transaction.TransactionId,
                TransactionDate = transaction.TransactionDate,
                Status = transaction.Status,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                InvoiceId = transaction.InvoiceId
            };
        }

    }
}
