using BE_SWP391.Data;
using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;

namespace BE_SWP391.Repositories.Implementations
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly EvMarketContext _context;
        public InvoiceRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Invoice? GetById(int id)
        {
            return _context.Invoices.Find(id);
        }
        public IEnumerable<Invoice> GetAll()
        {
            return _context.Invoices.ToList();
        }
        public void Create(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
        }
        public void Update(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            _context.SaveChanges();
        }
        public void Delete(Invoice invoice)
        {
                _context.Invoices.Remove(invoice);
                _context.SaveChanges();

        }


    }
}
