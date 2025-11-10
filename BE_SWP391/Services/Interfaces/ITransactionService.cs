using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface ITransactionService
    {


        List<RecentTransactionResponse> GetRecentTransactions(int count = 5);
        ReportTransactionResponse GetReportTransaction();

        PaymentCreateResponse CreatePaymentTransaction(PaymentRequest request);
        bool HandleCallbackVnPay(string queryString);
        bool HandleCallbackMomo(MomoCallbackRequest callback);
        TransactionStatusResponse CheckTransactionStatus(int transactionId);
        List<TransactionNowResponse> GetTransactionNow(int userId);
        List<TopBuyerResponse> GetTopBuyer(int userId);
        List<DataRevenueResponse> GetDataRevenueByUser(int userId);
    }
}
