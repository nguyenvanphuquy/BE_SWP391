using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
using BE_SWP391.Data;
using BE_SWP391.Repositories.Interfaces;
namespace BE_SWP391.Services.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public InvoiceResponse? GetById(int id)
        {
            var invoice = _invoiceRepository.GetById(id);
            return invoice == null ? null : ToResponse(invoice);
        }
        public IEnumerable<InvoiceResponse> GetAll()
        {
            return _invoiceRepository.GetAll().Select(i => ToResponse(i));
        }
        public InvoiceResponse Create(InvoiceRequest request)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = request.InvoiceNumber,
                IssueDate = request.IssueDate,
                DueDate = request.DueDate,
                TotalAmount = request.TotalAmount,
                Currency = request.Currency,
                TaxAmount = request.TaxAmount,
                UserId = request.UserId,
                Status = "Pending"
            };
            _invoiceRepository.Create(invoice);
            return ToResponse(invoice);
        }
        public InvoiceResponse? Update(int id, InvoiceRequest request)
        {
            var invoice = _invoiceRepository.GetById(id);
            if (invoice == null) return null;
            invoice.InvoiceNumber = request.InvoiceNumber;
            invoice.IssueDate = request.IssueDate;
            invoice.DueDate = request.DueDate;
            invoice.TotalAmount = request.TotalAmount;
            invoice.Currency = request.Currency;
            invoice.TaxAmount = request.TaxAmount;
            invoice.UserId = request.UserId;
            invoice.Status = request.Status;
            _invoiceRepository.Update(invoice);
            return ToResponse(invoice);
        }
        public bool Delete(int id)
        {
            var invoice = _invoiceRepository.GetById(id);
            if (invoice == null) return false;
            _invoiceRepository.Delete(invoice);
            return true;
        }
        public static InvoiceResponse ToResponse(Invoice invoice)
        {
            return new InvoiceResponse
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                TotalAmount = invoice.TotalAmount,
                Currency = invoice.Currency,
                TaxAmount = invoice.TaxAmount,
                UserId = invoice.UserId,
                Status = invoice.Status

            };
        }
    }

}
