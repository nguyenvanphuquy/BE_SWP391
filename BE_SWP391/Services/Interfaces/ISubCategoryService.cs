using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;

namespace BE_SWP391.Services.Interfaces
{
    public interface ISubCategoryService
    {
        SubCategoryResponse? GetById(int id);
        IEnumerable<SubCategoryResponse> GetAll();
        SubCategoryResponse? Create(SubCategoryRequest request);
        SubCategoryResponse? Update(int id, SubCategoryRequest request);
        bool Delete(int id);
    }
}
