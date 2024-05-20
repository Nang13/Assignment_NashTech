using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.Order;
using PES.Domain.DTOs.Product;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Common;

namespace PES.Application.IService
{
    public interface IProductService
    {
        Task<ProductResponse> AddNewProduct(AddNewProductRequest request);
        Task<ProductResponse> UpdateProduct(Guid id,Dictionary<string,object?> request);
        Task<Pagination<ProductsResponse>> GetProducts(GetProductRequest request);
        Task<ProductResponseDetail> GetProductDetail(Guid productId);

        Task<bool> DeleteProduct(Guid productId);

        Task AddRatingProduct(Guid id,ProductRatingRequest request);
    }
}