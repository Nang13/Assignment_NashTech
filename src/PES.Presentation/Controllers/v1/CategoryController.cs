using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.Category;

namespace PES.Presentation.Controllers.V1
{
    public class CategoryController : DefaultController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //todo Add New Category
        [HttpPost]
        public async Task<IActionResult> Add(AddNewCategoryRequest request)
        {
            await _categoryService.AddNewCategory(request);
            return Ok(new
            {
                message = "Create Successfully"
            });
        }


        //todo Get All MainCategories
        [HttpGet]
        public async Task<ActionResult> GetMainCategories()
        {
            var categories = await _categoryService.GetMainCategories();
            return Ok(categories);
        }


        //todo Get All Sub-Categories By CategoryId
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSubCategories(Guid id)
        {
            var categories = await _categoryService.GetSubCategories(id);
            return Ok(categories);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryService.UpdateCategory(id, request);
            return Ok(category);
        }

        

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            bool Delete = await _categoryService.DeleteCategory(id);
           return Ok(new
            {
                message = "Create Successfully"
            });
        }

    }
}