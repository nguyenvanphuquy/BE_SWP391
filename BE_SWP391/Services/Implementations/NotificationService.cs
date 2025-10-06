using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public NotificationResponse? GetById(int id)
        {
            var notification = _notificationRepository.GetById(id);
            return notification == null ? null : ToResponse(notification);
        }
        public IEnumerable<NotificationResponse> GetAll()
        {
            return _notificationRepository.GetAll().Select(n => ToResponse(n));
        }
        public NotificationResponse Create(NotificationRequest request)
        {
            var notification = new Notification
            {
                Title = request.Title,
                Message = request.Message,
                UserId = request.UserId,
                SentAt = DateTime.UtcNow,
                Status = "Sent"
            };
            _notificationRepository.Create(notification);
            return ToResponse(notification);
        }
        public NotificationResponse? Update(int id, NotificationRequest request)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification == null) return null;
            notification.Title = request.Title;
            notification.Message = request.Message;
            notification.UserId = request.UserId;
            notification.Status = request.Status;
            _notificationRepository.Update(notification);
            return ToResponse(notification);
        }
        public bool Delete(int id)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification == null) return false;
            _notificationRepository.Delete(notification);
            return true;
        }
        public static NotificationResponse ToResponse(Notification notification)
        {
            return new NotificationResponse
            {
                NotificationId = notification.NotificationId,   
                Title = notification.Title,
                Message = notification.Message,
                UserId = notification.UserId,
                SentAt = notification.SentAt,
                Status = notification.Status
            };
        }
        
    }
}
