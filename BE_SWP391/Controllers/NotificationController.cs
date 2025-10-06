using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<NotificationResponse>> GetAll()
        {
            var notifications = _notificationService.GetAll();
            return Ok(notifications);
        }
        [HttpGet("{id}")]
        public ActionResult<NotificationResponse> GetById(int id)
        {
            var notification = _notificationService.GetById(id);
            if (notification == null) return NotFound();
            return Ok(notification);
        }
        [HttpPost]
        public ActionResult<NotificationResponse> Create([FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var notification = _notificationService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = notification.NotificationId }, notification);
        }
        [HttpPut("{id}")]
        public ActionResult<NotificationResponse> Update(int id, [FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var notification = _notificationService.Update(id, request);
            if (notification == null) return NotFound();
            return Ok(notification);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = _notificationService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
