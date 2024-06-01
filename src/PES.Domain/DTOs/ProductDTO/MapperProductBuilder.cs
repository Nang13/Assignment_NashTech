using PES.Domain.Constant;
using PES.Domain.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.ProductDTO
{

    public static class MapperProductBuilder
    {
        public static Product ToDTO(this AddNewProductRequest request)
        {
            if (request != null)
            {
                //return new EmployeeForShortDto
                //{
                //    Id = employee.Id,
                //    EmpCode = employee.EmpCode,
                //    Fname = employee.Fname,
                //    Lname = employee.Lname,
                //    Age = NetCoreWebApplication1.Other.Extension.CalcAge(employee.DateBirth)
                //};

                return new Product
                {
                    ProductName = request.ProductName,
                    Price = request.Price,
                    Created = CurrentTime.RecentTime,
                    CategoryId = request.CategoryId,
                    Quantity = request.Quantity,
                    Description = request.Description
                };
            }

            return null;

        }


        public static ImportantInformation MapperDTO(this ImportantImformationRequest request)
        {
            if (request != null)
            {
                return new ImportantInformation
                {
                    Ingredients = request.Ingredients,
                    LegalDisclaimer = request.LegalDisclaimer,
                    Directions = request.Directions,
                };
            }

            return null;
        }

        public static NutrionInfo MapperNutrionDTO(this Product request)
        {
            if (request != null)
            {
                //return new NutrionInfo()
                //{
                //    Calories = request.NutritionInformation.Calories,
                //    Fiber = request.NutritionInformation.Fiber,
                //    Protein = request.NutritionInformation.Protein,
                //    Sodium = request.NutritionInformation.Sodium,   
                //    Sugars = request.NutritionInformation.Sugars,
                //};

                return new NutrionInfo(request.NutritionInformation.Calories, request.NutritionInformation.Fiber, request.NutritionInformation.Protein, request.NutritionInformation.Sodium, request.NutritionInformation.Sugars);
            }
            return null;
        }

        public static ImportantInfo MapperImportantDTO(this Product product)
        {
            if (product != null)
            {
                return new ImportantInfo(product.ImportantInformation.Ingredients, product.ImportantInformation.Directions, product.ImportantInformation.LegalDisclaimer);
            }
            return null;
        }

        public static ProductCategory MapperCategoryDTO(this Product product)
        {
            if(product != null)
            {
                return new ProductCategory(product.Category.Id, product.Category.CategoryName, product.Category.CategoryMain);
            }
            return null;
        }


         
    }


}
