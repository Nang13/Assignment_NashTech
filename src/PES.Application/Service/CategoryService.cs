using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Application.IService;
using PES.Domain.DTOs.CategoryDTO;
using PES.Domain.Entities.Model;
using PES.Domain.Enum;
using PES.Infrastructure.UnitOfWork;

namespace PES.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
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
        public async Task<CategoryDetailResponse> AddNewCategory(AddNewCategoryRequest request)
        {
            int rightValue = 0;
            Guid parentId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            if (request.AddType == (int)AddCategoryType.AddChildCategory)
            {

                //? checkParentId is exist
                var parentCategory1 = await _unitOfWork.CategoryRepository.GetByIdAsync((Guid)request.CategoryParentId) ?? throw new Exception("Not found Parent Category");
                parentId = (Guid)request.CategoryParentId;
                rightValue = parentCategory1.CategoryRight;
                request.CategoryMain = parentCategory1.CategoryMain;
                await _unitOfWork.CategoryRepository.UpdateCategoryParent((Guid)request.CategoryParentId, rightValue);
            }
            else
            {
                rightValue = 1;
            }
            var parentCategory = request.MapperDTO(rightValue,parentId);
/*            parentCategory.CategoryLeft = rightValue;
            parentCategory.CategoryRight = rightValue + 1;
            parentCategory.CategoryParentId = parentId;
            ? 1.Check CategoryMain is in system before
            var parentCategory = new Category()
            {
                CategoryLeft = rightValue,
                CategoryRight = rightValue + 1,
                CategoryMain = request.CategoryMain,
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription,
                CategoryParentId = parentId,
            };*/
            await _unitOfWork.CategoryRepository.AddAsync(parentCategory);
            await _unitOfWork.SaveChangeAsync();
            var categoryAdded = await _unitOfWork.CategoryRepository.FirstOrDefaultAsync(x => x.CategoryName == request.CategoryName);
          //  return new CategoryDetailResponse(categoryAdded.Id, categoryAdded.CategoryMain, categoryAdded.CategoryName, categoryAdded.CategoryLeft, categoryAdded.CategoryRight);

            return categoryAdded.MapperDTODetail();

        }




        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Not Found CategoryId");
            int leftValue = category.CategoryLeft;
            int rightValue = category.CategoryRight;
            int width = rightValue - leftValue + 1;
            throw new NotImplementedException();
        }

        public async Task<List<CategoryResponse>> GetMainCategories() => await _unitOfWork.CategoryRepository.GetAllParentCategory();


        public async Task<List<CategoryResponse>> GetSubCategories(Guid categoryParentId)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryParentId);
            if (category is not null)
            {
                var subCategory = _unitOfWork.CategoryRepository
                    .WhereAsync(x => x.CategoryMain == category.CategoryMain && x.CategoryLeft > category.CategoryLeft && x.CategoryRight <= category.CategoryRight)
                    .Result
                    .Select(x => x.MapperDTO()).ToList();
                var parentCategort = _unitOfWork.CategoryRepository.FirstOrDefaultAsync(x => x.Id == categoryParentId).Result;

                subCategory.Add(parentCategort.MapperDTO());
                return subCategory;

            }
            else
            {
                throw new Exception("Not find this categoryId");
            }
        }

        public async Task<CategoryResponse> UpdateCategory(Guid CategoryId, UpdateCategoryRequest request)
        {
            var categoryUpdate = await _unitOfWork.CategoryRepository.FirstOrDefaultAsync(x => x.Id == CategoryId);
            if (categoryUpdate != null)
            {
                categoryUpdate.CategoryName = request.CategoryName;
                categoryUpdate.CategoryDescription = request.CategoryDescription;
                _unitOfWork.CategoryRepository.Update(categoryUpdate);
                await _unitOfWork.SaveChangeAsync();


                return categoryUpdate.MapperDTO();
            }

            return null;
        }


    }
}