using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.Constant;
using PES.Domain.DTOs.Order;
using PES.Domain.DTOs.Product;
using PES.Domain.Entities.Model;
using PES.Infrastructure.UnitOfWork;

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

        public ProductService(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
        }
        public async Task<ProductResponse> AddNewProduct(AddNewProductRequest request)
        {
            Guid productId = Guid.NewGuid();
            Product product = new()
            {
                ProductName = request.ProductName,
                Price = request.Price,
                Id = productId,
                Created = CurrentTime.RecentTime
            };
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();

            var Task1 = AddInconsequentialImage(request.ListImages.Select(x => x.FileName).ToList(), productId);

            var Task2 = AddMainImage(request.MainImage.FileName, productId);

            await Task.WhenAll(Task1, Task2);
            await _unitOfWork.SaveChangeAsync();
            return new ProductResponse(productId, request.ProductName, product.Created);
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
            var product = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == productId);
            return new ProductResponseDetail(productId, product.ProductName, product.Price);
        }

        public async Task<List<Product>> GetProducts(GetProductRequest request)
        {
            var filterResult = request.Filter.Count > 0 ? [] : _unitOfWork.ProductRepository.GetAllAsync().Result.AsEnumerable();
            if (request.Filter!.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResult = filterResult.Union(FilterUtilities.SelectItems(filterResult, filter.Key, filter.Value));
                }
            }

            return PaginatedList<Product>.Create(
                source: filterResult.AsQueryable(),
                pageIndex: request.PageNumber,
                pageSize: request.PageSize
            );
        }

        public async Task<ProductResponse> UpdateProduct(Guid id, Dictionary<string, object?> request)
        {
            await _unitOfWork.ProductRepository.ExcuteUpdate(id, request);
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