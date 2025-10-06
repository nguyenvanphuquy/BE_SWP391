using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models;
using BE_SWP391.Repositories.Interfaces;

namespace BE_SWP391.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly EvMarketContext _context;
        public NotificationRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Notification? GetById(int id)
        {
            return _context.Notifications.Find(id);
        }
        public IEnumerable<Notification> GetAll()
        {
            return _context.Notifications.ToList();
        }
        public void Create(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }
        public void Update(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }
        public void Delete(Notification notification)
        {
                _context.Notifications.Remove(notification);
                _context.SaveChanges();
        }

    }
}
