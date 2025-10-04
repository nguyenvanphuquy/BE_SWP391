using Azure.Core;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using System.Collections.Generic;

namespace BE_SWP391.Services.Implementations
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        public SubCategoryService(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }
        public SubCategoryResponse? GetById(int id)
        {
            var subCategory = _subCategoryRepository.GetById(id);
            return subCategory == null ? null : ToResponse(subCategory);
        }
        public IEnumerable<SubCategoryResponse> GetAll()
        {
            return _subCategoryRepository.GetAll().Select(ToResponse);
        }

        public SubCategoryResponse? Create(SubCategoryRequest request)
        {
            var subCategory = new SubCategory
            {
                SubcategoryName = request.SubcategoryName,
                CategoryId = request.CategoryId,
                Description = request.Description,
                CreatedAt = DateTime.Now,
            };
            _subCategoryRepository.Create(subCategory);
            return ToResponse(subCategory);

             
        }
        public SubCategoryResponse? Update(int id, SubCategoryRequest request)
        {
            var subCategory = _subCategoryRepository.GetById(id);
            if (subCategory == null) return null;

            subCategory.SubcategoryName = request.SubcategoryName;
            subCategory.CategoryId = request.CategoryId;
            subCategory.Description = request.Description;
            _subCategoryRepository.Update(subCategory);
            return ToResponse(subCategory);
        }
        public bool Delete(int id)
        {
            var subCategory = _subCategoryRepository.GetById(id);
            if (subCategory == null) return false;
            _subCategoryRepository.Delete(subCategory);
            return true;
        }
        public static SubCategoryResponse ToResponse(SubCategory subCategory)
        {
            return new SubCategoryResponse
            {
                SubcategoryId = subCategory.SubcategoryId,
                SubcategoryName = subCategory.SubcategoryName,
                CategoryId = subCategory.CategoryId,
                Description = subCategory.Description,
                CreatedAt = subCategory.CreatedAt
            };
        }
    }
}
