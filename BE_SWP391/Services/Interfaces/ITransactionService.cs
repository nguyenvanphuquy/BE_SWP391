using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface ITransactionService
    {


        List<RecentTransactionResponse> GetRecentTransactions(int count = 5);
        ReportTransactionResponse GetReportTransaction();
        //PaymentCreateResponse CreatePayment(PaymentRequest request);
        string CreateTransaction(int userId, int[] cartIds, decimal totalAmount);
        bool HandleCallbackVnPay(IDictionary<string, string> query);
        bool HandleCallbackMomo(Dictionary<string, string> body);
    }
}
