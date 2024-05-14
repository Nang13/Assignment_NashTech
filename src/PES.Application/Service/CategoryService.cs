using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Application.IService;
using PES.Domain.DTOs.Category;

namespace PES.Application.Service
{
    public class CategoryService : ICategoryService
    {
        /*
        todo AddNewCategory
        + add Main-Category
        + add Sub-Category
        todo ViewCategory
        + view All MainCategory
        + view all Sub-Category
        + view all ProductInCategoryId
        todo DeleteCategory
        todo UpdateCategory
        */
        public Task<CategoryDetailResponse> AddNewCategory(AddNewCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryResponse>> GetMainCategories()
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryResponse>> GetSubCategories(Guid categoryParentId)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryResponse> UpdateCategory(Guid CategoryId, UpdateCategoryRequest request)
        {
            throw new NotImplementedException();
        }
    }
}