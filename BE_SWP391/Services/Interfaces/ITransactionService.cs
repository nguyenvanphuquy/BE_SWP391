using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface ITransactionService
    {
        TransactionResponse? GetById(int id);
        IEnumerable<TransactionResponse> GetAll();
        TransactionResponse? Create(TransactionRequest request);
        TransactionResponse? Update(int id, TransactionRequest request);
        bool Delete(int id);
        List<RecentTransactionResponse> GetRecentTransactions(int count = 5);
        ReportTransactionResponse GetReportTransaction();
    }
}
