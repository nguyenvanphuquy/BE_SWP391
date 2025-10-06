using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models;
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




    }
}
