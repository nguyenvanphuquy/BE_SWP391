using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface INotificationService
    {
        NotificationResponse? GetById(int id);
        IEnumerable<NotificationResponse> GetAll();
        NotificationResponse Create(NotificationRequest request);
        NotificationResponse? Update(int id, NotificationRequest request);
        bool Delete(int id);

    }
}
