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
                             ProviderName = user.FullName,
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
        public Transaction CreateTransaction(Transaction tx)
        {
            _context.Transactions.Add(tx);
            return tx;
        }
        public Invoice CreateInvoice(Invoice inv)
        {
            _context.Invoices.Add(inv);
            return inv;
        }

        public Transaction GetTransactionById(int id)
            => _context.Transactions.FirstOrDefault(t => t.TransactionId == id);

        public void UpdateTransaction(Transaction tx)
            => _context.Transactions.Update(tx);

        public IEnumerable<Cart> GetCartsByIds(int[] ids)
            => _context.Carts.Where(c => ids.Contains(c.CartId)).ToList();

        public void RemoveCart(Cart c) => _context.Carts.Remove(c);

        public void SaveChanges() => _context.SaveChanges();
    }
}
