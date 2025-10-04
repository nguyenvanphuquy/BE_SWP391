using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models.DTOs.Request;
namespace BE_SWP391.Services.Interfaces
{
    public interface ICategoryService
    {
        CategoryResponse? GetById(int id);
        IEnumerable<CategoryResponse> GetAll();
        CategoryResponse? Create(CategoryRequest request);
        CategoryResponse? Update(int id,CategoryRequest request);
        bool Delete(int id);
    }
}
