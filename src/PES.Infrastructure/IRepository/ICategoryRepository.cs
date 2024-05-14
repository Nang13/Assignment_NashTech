using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.Category;
using PES.Domain.Entities.Model;

namespace PES.Infrastructure.IRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
         public  Task UpdateCategoryParent(Guid categoryId, int rightValue);

         public Task<List<CategoryResponse>> GetAllParentCategory();

         public  Task DeleteAndUpdateSubCategories(string CategoryMain, int leftValue, int rightValue, int width);
    }
}