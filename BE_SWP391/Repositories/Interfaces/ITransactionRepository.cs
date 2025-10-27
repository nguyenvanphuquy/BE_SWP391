using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction? GetById(int id);
        IEnumerable<Transaction> GetAll();
        void Create(Transaction transaction);
        void Update(Transaction transaction);
        void Delete(Transaction transaction);
        List<RecentTransactionResponse> GetRecentTransaction(int count = 5);
        ReportTransactionResponse GetReportTransaction();
    }
}
