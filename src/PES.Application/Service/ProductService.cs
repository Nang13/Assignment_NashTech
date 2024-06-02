using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Api.Gax.ResourceNames;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.Constant;
using PES.Domain.DTOs.Order;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Common;
using PES.Infrastructure.UnitOfWork;
using static System.Net.Mime.MediaTypeNames;

namespace PES.Application.Service
{
    public class ProductService : IProductService
    {
        /*
        todo viewProduct
        todo viewProductDetail
        todo addNewProduct
        todo updateNewProduct

        */
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        private readonly ICategoryService _categoryService;

        public ProductService(IUnitOfWork unitOfWork, IClaimsService claimsService, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
            _categoryService = categoryService;
        }
        public async Task<ProductResponse> AddNewProduct(AddNewProductRequest request)
        {
            request.MainImage = request.ListImages.FirstOrDefault();
            request.ListImages.RemoveAt(0);
            Guid productId = Guid.NewGuid();
            //Product product = new()
            //{
            //    ProductName = request.ProductName,
            //    Price = request.Price,
            //    Id = productId,
            //    Created = CurrentTime.RecentTime,
            //    CategoryId = request.CategoryId,
            //    Quantity = request.Quantity,
            //    Description = request.Description
            //};
            Product product = request.ToDTO();
            product.Id = productId;


            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();

            if (request.ListImages.Count >= 1)
            {
                var Task1 = AddInconsequentialImage(request.ListImages, productId,request.ProductName);
                var Task2 = AddMainImage(request.MainImage, productId,request.ProductName);
                await Task.WhenAll(Task1, Task2);
            }
            else
            {
                await AddMainImage(request.MainImage, productId,request.ProductName);
            }


            if (request.NutrionInforrmationRequest is not null) await AddNutrionInformation(request.NutrionInforrmationRequest, productId);
            if (request.ImformationRequest is not null) await AddImportantInformation(request.ImformationRequest, productId);


            await _unitOfWork.SaveChangeAsync();
            return new ProductResponse(productId, request.ProductName, product.Created);
        }

        public async Task AddInconsequentialImage(string Images, Guid productId,string productName)
        {
            var productImage = new ProductImage
            {
                ImageUrl = Images+ Regex.Replace(productName, @"\s", ""),   // Assuming the image string is the path
                ProductId = productId,
                IsMain = false
            };
            await _unitOfWork.ProductImageRepository.AddAsync(productImage);

        }


        public async Task AddInconsequentialImage(List<string> Images, Guid productId, string productName)
        {
            var productImages = Images.Select(image => new ProductImage
            {
                ImageUrl = image + Regex.Replace(productName, @"\s", ""),   // Assuming the image string is the path
                ProductId = productId,
                IsMain = false
            }).ToList();
            await _unitOfWork.ProductImageRepository.AddRangeAsync(productImages);

        }

        public async Task AddNutrionInformation(NutrionInforrmationRequest request, Guid ProductId)
        {
            NutritionInformation nutrition = new()
            {
                Calories = request.Calories,
                Fiber = request.Fiber,
                Protein = request.Protein,
                Sodium = request.Sodium,
                Sugars = request.Sugars,
                ProductId = ProductId,
                Created = DateTime.UtcNow.AddHours(7),

            };
            await _unitOfWork.NutrionInfoRepository.AddAsync(nutrition);
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task AddImportantInformation(ImportantImformationRequest request, Guid ProductId)
        {
            //ImportantInformation information = new()
            //{
            //    Ingredients = request.Ingredients,
            //    LegalDisclaimer = request.LegalDisclaimer,
            //    Directions = request.Directions,
            //    ProductId = ProductId
            //};
            ImportantInformation information = request.MapperDTO();
            information.ProductId = ProductId;
            await _unitOfWork.ImportantInfoRepository.AddAsync(information);
            await _unitOfWork.SaveChangeAsync();
        }




        public async Task AddMainImage(string Images, Guid productId,string ProductName)
        {
            await _unitOfWork.ProductImageRepository.AddAsync(new ProductImage
            {
                ImageUrl = Images+ Regex.Replace(ProductName, @"\s", ""),
                ProductId = productId,
                IsMain = true,
            });
            //  await _unitOfWork.SaveChangeAsync();
        }

        public async Task<ProductResponseDetail> GetProductDetail(Guid productId)
        {
            var product = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == productId, x => x.ImportantInformation, x => x.NutritionInformation, x => x.ProductImages, x => x.Category);

            var nutritionInfo = product.NutritionInformation != null ? product.MapperNutrionDTO() : null;

            var importantInfo = product.ImportantInformation != null ? product.MapperImportantDTO() : null;

            var productImage = product.ProductImages != null ? product.ProductImages.Select(x => new ProductImageResponse(x.ImageUrl, x.IsMain)).ToList() : null;

            return new ProductResponseDetail(productId, product.ProductName, product.Price, nutritionInfo, product.MapperCategoryDTO(), importantInfo, productImage);
        }

        //? Search by CategoryName
        public async Task<List<Guid>> GetCategoryIdByCategoryName(string categoryName)
        {
            List<Guid> categoryContains = _unitOfWork.CategoryRepository.WhereAsync(x => x.CategoryName.Contains(categoryName)).Result.Select(x => x.Id).ToList();
            return categoryContains;
        }

        //? Search by CategoryMain
        public async Task<List<Guid>> GetCategoryIdByCategoryMain(string categoryMain)
        {
            List<Guid> categoryContains = _unitOfWork.CategoryRepository.WhereAsync(x => x.CategoryMain.Contains(categoryMain)).Result.Select(x => x.Id).ToList();
            return categoryContains;
        }
        //? Search by productName 
        public async Task<List<Guid>> GetProductByProductName(string categoryMain)
        {
            List<Guid> categoryContains = _unitOfWork.CategoryRepository.WhereAsync(x => x.CategoryMain.Contains(categoryMain)).Result.Select(x => x.Id).ToList();
            return categoryContains;
        }
        public async Task<Pagination<ProductsResponse>> GetProducts(GetProductRequest request)
        {
            List<Guid> subCategories = [];
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");



            //todo get categoryId by name 
            if (request.Filter.ContainsKey("CategoryId"))
            {

                subCategories = _categoryService.GetSubCategories(Guid.Parse(request.Filter.GetValueOrDefault("CategoryId"))).Result.Select(x => x.CategoryId).ToList();

            }

            //  var filterResult = request.Filter.Count > 0 ? [] : _unitOfWork.ProductRepository.GetAllAsync().Result.AsEnumerable();
            var filterResult = _unitOfWork.ProductRepository.GetAllAsync(x => x.Category, x => x.ProductImages).Result.AsEnumerable();
            var testingData = _unitOfWork.ProductRepository.GetAllAsync(x => x.Category, x => x.ProductImages).Result.Select(x => new ProductsResponse(x.Id, x.ProductName, x.Created, x.ProductImages.FirstOrDefault().ImageUrl, x.Category.CategoryMain, x.Category.CategoryName, x.Price, x.Description, x.CategoryId)).AsEnumerable();



            /*            if (!subCategories.IsNullOrEmpty())
                        {
                            foreach (var category in subCategories)
                            {
                                filterResult = filterResult.Union(FilterUtilities.SelectItems(filterResult, "CategoryId", category.ToString()));
                            }
                        }
                        if (request.Filter!.Count > 0)
                        {
                            foreach (var filter in request.Filter)
                            {
                                var checker = FilterUtilities.SelectId(filterResult, filter.Key, filter.Value).Select(x => x.Id).ToList();
                                filterResult = filterResult.Where(x => checker.Contains(x.Id));
                                Console.WriteLine(filterResult);
                            }
                        }*/
            var filterResultTesting = request.Filter.Count > 0 ? new List<ProductsResponse>() : testingData;
            HashSet<Guid> categoryName = [];
            ////todo categoryName or categoryMain is not null
            if (request.Filter.ContainsKey("CategoryName"))
            {
                categoryName = categoryName.Union(await GetCategoryIdByCategoryMain(request.Filter.GetValueOrDefault("CategoryName"))).ToHashSet();
                request.Filter!.Remove("CategoryName");
            }

            if (request.Filter.ContainsKey("CategoryMain"))
            {
                categoryName = categoryName.Union(await GetCategoryIdByCategoryMain(request.Filter.GetValueOrDefault("CategoryMain"))).ToHashSet();
                request.Filter!.Remove("CategoryMain");

            }

            filterResultTesting = filterResultTesting.Union(testingData.Where(x => categoryName.Contains(x.CategoryId)));

            if (request.Filter?.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResultTesting = filterResultTesting.Union(FilterUtilities.SelectItems(testingData, filter.Key, filter.Value));
                }
            }

            var dataCheck = new Pagination<ProductsResponse>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                TotalItemsCount = filterResult.Count(),
                Items = PaginatedList<ProductsResponse>.Create(
                       source: filterResultTesting.AsQueryable(),
                       pageIndex: request.PageNumber,
                       pageSize: request.PageSize)


            };
            // Serialize the result with reference handling


            return dataCheck;
        }

        public async Task<ProductResponse> UpdateProduct(Guid id, UpdateProductRequest request)
        {
            if (request.importantInfo is not null)
            {
                var Imrequest = await _unitOfWork.ImportantInfoRepository.FirstOrDefaultAsync(x => x.ProductId == id);
                if (Imrequest is null)
                {
                    await AddImportantInformation(request.importantInfo, id);
                }
                else
                {
                    Imrequest.Ingredients = request.importantInfo.Ingredients;
                    Imrequest.Directions = request.importantInfo.Directions;
                    Imrequest.LegalDisclaimer = request.importantInfo.LegalDisclaimer;
                    _unitOfWork.ImportantInfoRepository.Update(Imrequest);
                }

            }
            if (request.nutrionInfo is not null)
            {
                var NuRequest = await _unitOfWork.NutrionInfoRepository.FirstOrDefaultAsync(x => x.ProductId == id);
                if (NuRequest is null)
                {
                    await AddNutrionInformation(request.nutrionInfo, id);
                }
                else
                {
                    NuRequest.Sodium = request.nutrionInfo.Sodium;
                    NuRequest.Sugars = request.nutrionInfo.Sugars;
                    NuRequest.Protein = request.nutrionInfo.Protein;
                    NuRequest.Calories = request.nutrionInfo.Calories;
                    NuRequest.Fiber = request.nutrionInfo.Fiber;
                    _unitOfWork.NutrionInfoRepository.Update(NuRequest);
                }

            }

            //? not processing image add and delete and have element = 
            if (!request.productImages.IsNullOrEmpty())
            {
                List<ProductImage> productImages = await _unitOfWork.ProductImageRepository.WhereAsync(x => x.ProductId == id);
                string productName =  _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == id).Result.ProductName;  
                //? in Database    1  2   3
                //? input Delete   1   2
                foreach (var item in request.productImages)
                {
                    ProductImage image = await _unitOfWork.ProductImageRepository.FirstOrDefaultAsync(x => x.ImageUrl == item);
                    //? if not found: New Image One
                    if (image is null)
                    {
                        await AddInconsequentialImage(item, id,productName);
                    }
                    else
                    {
                        //? remove element 
                        //? after finish this if the loop have element it will be deleted
                        productImages.Remove(image);
                    }
                }
                if (productImages is not null)
                {
                    await _unitOfWork.ProductImageRepository.DeleteRange(productImages);
                }


                //? input Adding 1 2 3 4
            }
            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.ProductRepository.ExcuteUpdate(id, request.ObjectUpdate);
            return new ProductResponse(Guid.NewGuid(), "com suon hoc mon", DateTime.UtcNow);
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            await _unitOfWork.ProductRepository.ExcuteUpdate(productId, new Dictionary<string, object?>() {
               { "Inactive", nameof(Product.Status) },
            });
            return true;
        }

        public async Task AddRatingProduct(Guid productId, ProductRatingRequest request)
        {
            string userId = _claimsService.GetCurrentUserId;
            ProductRating productRating = new ProductRating()
            {
                Rating = (int)request.Rating,
                ProductId = productId,
                UsefulComment = 0,
                UserId = userId,
                Comment = request.Comment
            };
            await _unitOfWork.ProductRatingRepository.AddAsync(productRating);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}