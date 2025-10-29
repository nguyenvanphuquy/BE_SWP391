using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_SWP391.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly EvMarketContext _context;
        public UserRepository(EvMarketContext context)
        {
            _context = context;
        }
        public User? GetById(int id) => _context.Users.Find(id);
        public IEnumerable<User> GetAll() => _context.Users.ToList();
        public IEnumerable<User> GetAllInfor() => _context.Users.Include(u => u.Role).Include(u => u.DataPackages).ToList();
        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        public User? GetByEmail(string email) => _context.Users.FirstOrDefault(u => u.Email == email);
        public void UpdateStatus(int id, string status)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.Status = status;
                _context.SaveChanges();
            }
        }
        public User? GetByUsername(string username)
            => _context.Users.FirstOrDefault(u => u.Username == username && u.Status == "Active");
    }
}
