using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PaymentMethodResponse>> GetAll()
        {
            var paymentMethods = _paymentMethodService.GetAll();
            return Ok(paymentMethods);
        }
        [HttpGet("{id}")]
        public ActionResult<PaymentMethodResponse> GetById(int id)
        {
            var paymentMethod = _paymentMethodService.GetById(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return Ok(paymentMethod);
        }
        [HttpPost]
        public ActionResult<PaymentMethodResponse> Create([FromBody] PaymentMethodRequest request)
        {
            var paymentMethod = _paymentMethodService.Create(request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(GetById), new { id = paymentMethod.MethodId }, paymentMethod);
        }
        [HttpPut("{id}")]
        public ActionResult<PaymentMethodResponse> Update(int id, [FromBody] PaymentMethodRequest request)
        {
            var paymentMethod = _paymentMethodService.Update(id, request);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(paymentMethod);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = _paymentMethodService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
