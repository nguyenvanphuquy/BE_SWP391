using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet("{id}")]
        public ActionResult<FeedbackResponse> GetById(int id)
        {
            var feedback = _feedbackService.GetById(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }
        [HttpGet]
        public ActionResult<IEnumerable<FeedbackResponse>> GetAll()
        {
            var feedbacks = _feedbackService.GetAll();
            return Ok(feedbacks);
        }
        [HttpPost]
        public ActionResult<FeedbackResponse> Create([FromBody] FeedbackRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var feedback = _feedbackService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = feedback.FeedbackId }, feedback);
        }
        [HttpPut("{id}")]
        public ActionResult<FeedbackResponse> Update(int id, [FromBody] FeedbackRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var feedback = _feedbackService.Update(id, request);
            if (feedback == null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _feedbackService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
