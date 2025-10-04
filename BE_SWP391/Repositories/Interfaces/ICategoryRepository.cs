using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Category? GetById(int id);
        IEnumerable<Category> GetAll();
        void Create(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
