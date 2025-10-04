using BE_SWP391.Models.Entities;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using System.Linq;


namespace BE_SWP391.Services.Implementations
{
    public class CategorySevice : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategorySevice(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public CategoryResponse? GetById(int id)
        {
            var category = _categoryRepository.GetById(id);
            return category == null ? null : ToResponse(category);
        }
        public IEnumerable<CategoryResponse> GetAll()
        {
            return _categoryRepository.GetAll().Select(ToResponse);
        }
        public CategoryResponse? Create (CategoryRequest request)
        {
            var category = new Category
            {
                CategoryName = request.CategoryName,
                Description = request.Description,
                CreatedAt = DateTime.Now
            };
            _categoryRepository.Create(category);
            return ToResponse(category);
        }
        public CategoryResponse? Update(int id, CategoryRequest request)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                category.CategoryName = request.CategoryName;
            }
            if (!string.IsNullOrEmpty(request.Description))
            {
                category.Description = request.Description;
            }
            _categoryRepository.Update(category);
            return ToResponse(category);

        }
        public bool Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return false;
            }
            _categoryRepository.Delete(category);
            return true;
        }
        public static CategoryResponse ToResponse(Category category)
        {
            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                CreatedAt = category.CreatedAt
            };
        }
    }
}
