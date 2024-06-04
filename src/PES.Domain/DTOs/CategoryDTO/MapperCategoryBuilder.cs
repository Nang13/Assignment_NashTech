using PES.Domain.DTOs.CategoryDTO;
using PES.Domain.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.CategoryDTO
{
    public static class MapperCategoryBuilder
    {
        public static CategoryResponse MapperDTO(this Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return new CategoryResponse
            {
                CategoryDescription = category.CategoryDescription,
                CategoryId = category.Id,
                CategoryName = category.CategoryName,
                Left = category.CategoryLeft,
                Right = category.CategoryRight,
                ParentId = category.CategoryParentId
            };
        }


        public static Category MapperDTO(this AddNewCategoryRequest request, int rightValue, Guid ParentId)
        {

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new Category
            {
                CategoryMain = request.CategoryMain,
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription,
                CategoryParentId = ParentId,
                CategoryRight = rightValue,
                CategoryLeft = rightValue + 1

            };
        }


        public static CategoryDetailResponse MapperDTODetail(this Category category)
        {

            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            return new CategoryDetailResponse(category.Id,category.CategoryMain,category.CategoryName,category.CategoryLeft,category.CategoryRight);
        }
    }
}
