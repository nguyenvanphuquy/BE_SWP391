using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Notification? GetById(int id);
        IEnumerable<Notification> GetAll();
        void Create(Notification notification);
        void Update(Notification notification);
        void Delete(Notification notification);
    }
}
