using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface ITransactionRepository
    {


        List<RecentTransactionResponse> GetRecentTransaction(int count = 5);
        ReportTransactionResponse GetReportTransaction();
        Transaction CreateTransaction(Transaction tx);
        Invoice CreateInvoice(Invoice inv);
        Transaction GetTransactionById(int id);
        void UpdateTransaction(Transaction tx);
        IEnumerable<Cart> GetCartsByIds(int[] ids);
        void RemoveCart(Cart c);
        void SaveChanges();
    }
}
