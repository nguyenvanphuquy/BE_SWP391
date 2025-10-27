    using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Repositories.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly EvMarketContext _context;
        public TransactionRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Transaction? GetById(int id)
        {
            return _context.Transactions.Find(id);
        }
        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }
        public void Create(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
        public void Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }
        public void Delete(Transaction transaction)
        {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
        }
        public List<RecentTransactionResponse> GetRecentTransaction(int count = 5)
        {
            var query = (from t in _context.Transactions
                         join inv in _context.Invoices on t.InvoiceId equals inv.InvoiceId
                         join user in _context.Users on inv.UserId equals user.UserId
                         join rs in _context.RevenueShares on t.TransactionId equals rs.TransactionId into rsJoin
                         from rs in rsJoin.DefaultIfEmpty()
                         join dp in _context.DataPackages on user.UserId equals dp.UserId
                         join pp in _context.PricingPlans on dp.PackageId equals pp.PackageId
                         orderby t.TransactionDate descending
                         select new RecentTransactionResponse
                         {
                             TransactionId = t.TransactionId,
                             ProviderName = user.Username,
                             PackageName = dp.PackageName,
                             Amount = t.Amount,
                             Commission = rs != null ? rs.Amount : 0,
                             TransactionDate = t.TransactionDate,
                             Status = t.Status
                         })
                         .Take(count)
                         .ToList();

            return query;
        }
        public ReportTransactionResponse GetReportTransaction()
        {
            var total = _context.Transactions.Count();
            if (total == 0)
            {
                return new ReportTransactionResponse
                {
                    TotalTransaction = 0,
                    TotalAmount = 0,
                    AverageTransaction = 0,
                    SuccessPercentage = 0,
                    FailurePercentage = 0,
                    PendingPercentage = 0,
                };
            }
            var totalAmount =  _context.Transactions.Sum(t => t.Amount);
            var successCount = _context.Transactions.Count(t => t.Status == "completed");
            var failureCount = _context.Transactions.Count(t => t.Status == "failed");
            var pendingCount = _context.Transactions.Count(t => t.Status == "pending");
            var firstTransactionDate = _context.Transactions.OrderBy(t => t.TransactionDate)
                                                .Select(t => t.TransactionDate)
                                                .FirstOrDefault();
            return new ReportTransactionResponse
            {
                TotalTransaction = total,
                TotalAmount= totalAmount,
                AverageTransaction = Math.Round((double)totalAmount / total, 2),
                SuccessPercentage = Math.Round((double)successCount * 100 / total, 2),
                FailurePercentage = Math.Round((double)failureCount * 100 / total, 2),
                PendingPercentage = Math.Round((double)pendingCount * 100 / total, 2),

            };
        }
    }
}
