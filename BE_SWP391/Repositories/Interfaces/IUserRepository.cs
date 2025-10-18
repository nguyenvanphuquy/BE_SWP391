using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetById(int id);
        IEnumerable<User> GetAll();
        IEnumerable<User> GetAllInfor();
        void Create(User user);
        void Register(User user);
        void Update(User user);
        void Delete(int id);
        User? GetByEmail(string email);
        void UpdateStatus(int id, string status);
        User? GetByUsername(string username);

    }
}
