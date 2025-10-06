using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
namespace BE_SWP391.Services.Interfaces
{
    public interface IInvoiceService
    {
        InvoiceResponse? GetById(int id);
        IEnumerable<InvoiceResponse> GetAll();
        InvoiceResponse Create(InvoiceRequest request);
        InvoiceResponse? Update(int id, InvoiceRequest request);
        bool Delete(int id);
    }
}
