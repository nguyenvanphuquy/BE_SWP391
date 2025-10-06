using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<InvoiceResponse>> GetAll()
        {
            var invoices = _invoiceService.GetAll();
            return Ok(invoices);
        }
        [HttpGet("{id}")]
        public ActionResult<InvoiceResponse> GetById(int id)
        {
            var invoice = _invoiceService.GetById(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }
        [HttpPost]
        public ActionResult<InvoiceResponse> Create([FromBody] InvoiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var invoice = _invoiceService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = invoice.InvoiceId }, invoice);
        }
        [HttpPut("{id}")]
        public ActionResult<InvoiceResponse> Update(int id, [FromBody] InvoiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var invoice = _invoiceService.Update(id, request);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = _invoiceService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
