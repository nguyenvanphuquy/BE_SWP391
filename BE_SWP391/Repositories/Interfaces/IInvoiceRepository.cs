using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        Invoice? GetById(int id);
        IEnumerable<Invoice> GetAll();
        void Create(Invoice invoice);
        void Update(Invoice invoice);
        void Delete(Invoice invoice);
    }
}
