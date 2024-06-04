using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.CategoryDTO;

namespace PES.Application.IService
{
    public interface ICategoryService
    {
        Task<CategoryDetailResponse> AddNewCategory(AddNewCategoryRequest request);

        Task<List<CategoryResponse>> GetMainCategories();

        Task<List<CategoryResponse>> GetSubCategories(Guid categoryParentId);

        Task<bool> DeleteCategory(Guid categoryId);

        Task<CategoryResponse> UpdateCategory(Guid CategoryId, UpdateCategoryRequest request);
    }
}