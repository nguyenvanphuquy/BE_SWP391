using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var transactions = _transactionService.GetAll();
            return Ok(transactions);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var transaction = _transactionService.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
        [HttpPost]
        public IActionResult Create([FromBody] TransactionRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var transaction = _transactionService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = transaction.TransactionId }, transaction);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var transaction = _transactionService.Update(id, request);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _transactionService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("GetRecent")]
        public IActionResult GetRecentTransaction(int count = 5)
        {
            return Ok(_transactionService.GetRecentTransactions());
        }
        [HttpGet("ReportTransaction")]
        public IActionResult GetReportTransaction()
        {
            return Ok(_transactionService.GetReportTransaction());
        }
    }
}
