using BE_SWP391.Models;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface ISubCategoryRepository
    {
        SubCategory? GetById(int id);
        IEnumerable<SubCategory> GetAll();
        void Create(SubCategory subCategory);
        void Update(SubCategory subCategory);
        void Delete(SubCategory subCategory);
        SubCategory GetByName(string name);
    }
}
