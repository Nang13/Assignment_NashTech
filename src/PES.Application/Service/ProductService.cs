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
using PES.Application.Helper.ErrorHandler;
using PES.Application.Helper.RedisHandler;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.Constant;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.Domain.Enum;
using PES.Infrastructure.Common;
using PES.Infrastructure.UnitOfWork;
using StackExchange.Redis;
using static System.Net.Mime.MediaTypeNames;
using Role = PES.Domain.Enum.Role;

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
        private readonly IDatabase _dataabse;

        public ProductService(IUnitOfWork unitOfWork, IClaimsService claimsService, ICategoryService categoryService, IDatabase database)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
            _categoryService = categoryService;
            _dataabse = database;
        }
        public async Task<ProductResponse> AddNewProduct(AddNewProductRequest request)
        {
            //? check name is existed 
            if (!await CheckDuplicateProductName(request.ProductName)) throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "This Name already used in system");


            request.MainImage = request.ListImages.FirstOrDefault();
            request.ListImages.RemoveAt(0);
            Guid productId = Guid.NewGuid();
            Product product = request.ToDTO();
            product.Id = productId;
            product.IsDeleted = false;

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();

            if (request.ListImages.Count >= 1)
            {
                var Task1 = AddInconsequentialImage(request.ListImages, productId);
                var Task2 = AddMainImage(request.MainImage, productId);
                await Task.WhenAll(Task1, Task2);
            }
            else
            {
                await AddMainImage(request.MainImage, productId);
            }


            if (request.NutrionInforrmationRequest is not null) await AddNutrionInformation(request.NutrionInforrmationRequest, productId);
            if (request.ImformationRequest is not null) await AddImportantInformation(request.ImformationRequest, productId);


            await _unitOfWork.SaveChangeAsync();
            return new ProductResponse(productId, request.ProductName, product.Created);
        }

      

        private async Task<bool> CheckDuplicateProductName(string productName)
        {
            if (_unitOfWork.ProductRepository.WhereAsync(x => x.ProductName == productName).Result.IsNullOrEmpty())
            {
                return true;
            }
            return false;
        }

        public async Task AddInconsequentialImage(string Images, Guid productId)
        {
            var productImage = new ProductImage
            {
                ImageUrl = Images, // Assuming the image string is the path
                ProductId = productId,
                IsMain = false
            };
            await _unitOfWork.ProductImageRepository.AddAsync(productImage);

        }


        public async Task AddInconsequentialImage(List<string> Images, Guid productId)
        {
            var productImages = Images.Select(image => new ProductImage
            {
                ImageUrl = image,   // Assuming the image string is the path
                ProductId = productId,
                IsMain = false
            }).ToList();
            await _unitOfWork.ProductImageRepository.AddRangeAsync(productImages);

        }

        public async Task AddNutrionInformation(NutrionInforrmationRequest request, Guid ProductId)
        {
            NutritionInformation nutrition = request.MapDTO(ProductId);
            await _unitOfWork.NutrionInfoRepository.AddAsync(nutrition);
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task AddImportantInformation(ImportantImformationRequest request, Guid ProductId)
        {
            ImportantInformation information = request.MapperDTO(ProductId);
            await _unitOfWork.ImportantInfoRepository.AddAsync(information);
            await _unitOfWork.SaveChangeAsync();
        }




        public async Task AddMainImage(string Images, Guid productId)
        {
            await _unitOfWork.ProductImageRepository.AddAsync(new ProductImage
            {
                ImageUrl = Images,
                ProductId = productId,
                IsMain = true,
            });
            //  await _unitOfWork.SaveChangeAsync();
        }

        public async Task<ProductResponseDetail> GetProductDetail(Guid productId)
        {
            if (_claimsService.UserRole == Domain.Enum.Role.User) Task.Run(() => ViewProductHandlers(_claimsService.GetCurrentUserId, productId));


            var product = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == productId, x => x.ImportantInformation, x => x.NutritionInformation, x => x.ProductImages, x => x.Category);

            var nutritionInfo = product.NutritionInformation != null ? product.MapperNutrionDTO() : null;

            var importantInfo = product.ImportantInformation != null ? product.MapperImportantDTO() : null;

            var productImage = product.ProductImages?.Select(x => new ProductImageResponse(x.ImageUrl, x.IsMain)).ToList();
            var ratingProduct = _unitOfWork.ProductRatingRepository.WhereAsync(x => x.ProductId == productId, x => x.User).Result.Select(x => x.MapDTO()).ToList();

            return new ProductResponseDetail(productId, product.ProductName, product.Quantity, product.Price, product.Status, product.IsDeleted, product.Description, nutritionInfo, product.MapperCategoryDTO(), importantInfo, productImage, ratingProduct);
        }

        public async ValueTask ViewProductHandlers(string UserId, Guid ProductId)
        {
            using var objectT = new PopularProductHandler(_dataabse);
            await objectT.ProductVoting(UserId, ProductId, 1);
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


            var filterResult = _unitOfWork.ProductRepository.GetAllAsync(x => x.Category, x => x.ProductImages, x => x.ProductRating).Result.AsEnumerable();
            var testingData = _unitOfWork.ProductRepository.GetAllAsync(x => x.Category, x => x.ProductImages, x => x.ProductRating).Result.Select(x => new ProductsResponse(x.Id, x.ProductName, x.ProductRating.Any() ? x.ProductRating.Average(r => r.Rating) : 0, x.Created, x.ProductImages.FirstOrDefault().ImageUrl, x.Category.CategoryMain, x.Category.CategoryName, x.Price, x.Description, x.CategoryId, x.Status, (bool)x.IsDeleted)).AsEnumerable();


            var filterResultTesting = request.Filter.Count > 0 ? new List<ProductsResponse>() : testingData;

            HashSet<Guid> categoryName = [];
            //todo categoryName or categoryMain is not null
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
            if (request.Filter.ContainsKey("PopularProduct"))
            {

                using var redis = new PopularProductHandler(_dataabse);
                SortedSetEntry[] entries = await redis.ProductRatingList(request.Filter?.GetValueOrDefault("PopularProduct"));
                foreach (var item in entries.OrderByDescending(x => x.Score))
                {
                    filterResultTesting = filterResultTesting.Union(testingData.Where(x => x.Id == Guid.Parse(item.Element.ToString())));
                }
                request.Filter!.Remove("PopularProduct");

            }
            if (request.Filter?.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResultTesting = filterResultTesting.Union(FilterUtilities.SelectItems(testingData, filter.Key, filter.Value));
                }
            }


            Pagination<ProductsResponse> dataCheck = new();

            if (_claimsService.UserRole != Role.Administrator)
            {
                dataCheck = new Pagination<ProductsResponse>
                {
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItemsCount = filterResult.Count(),
                    Items = PaginatedList<ProductsResponse>.Create(
                  source: filterResultTesting.Where(x => x.isDeleted is false).AsQueryable(),
                  pageIndex: request.PageNumber,
                  pageSize: request.PageSize)


                };
            }
            else
            {
                dataCheck = new Pagination<ProductsResponse>
                {
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItemsCount = filterResult.Count(),
                    Items = PaginatedList<ProductsResponse>.Create(
                  source: filterResultTesting.Where(x => x.Status == ProductState.InStock).AsQueryable(),
                  pageIndex: request.PageNumber,
                  pageSize: request.PageSize)

                    
                };
            }

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

                    Imrequest = request.importantInfo.MapperDTO(id);
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
                    NuRequest = request.nutrionInfo.MapDTO(id);
                    _unitOfWork.NutrionInfoRepository.Update(NuRequest);
                }

            }

            //? not processing image add and delete and have element = 
            if (!request.productImages.IsNullOrEmpty())
            {
                List<ProductImage> productImages = await _unitOfWork.ProductImageRepository.WhereAsync(x => x.ProductId == id);

                //? in Database    1  2   3
                //? input Delete   1   2
                foreach (var item in request.productImages)
                {
                    ProductImage image = await _unitOfWork.ProductImageRepository.FirstOrDefaultAsync(x => x.ImageUrl == item);
                    //? if not found: New Image One
                    if (image is null)
                    {
                        await AddInconsequentialImage(item, id);
                    }
                    else
                    {
                        //? remove element 
                        //? after finish this if the loop have element it will be deleted

                        productImages.Remove(productImages.FirstOrDefault(x => x.ImageUrl == item));
                    }
                }
                if (!productImages.IsNullOrEmpty())
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

        public async Task ActiveProdudct(Guid productId)
        {
            Product product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            product.IsDeleted = false;
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task InactiveProduct(Guid productId)
        {
            Product product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            product.IsDeleted = true;
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}