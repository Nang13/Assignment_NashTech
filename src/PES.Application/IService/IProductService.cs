using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Common;

namespace PES.Application.IService
{
    public interface IProductService
    {
        Task<ProductResponse> AddNewProduct(AddNewProductRequest request);
        Task<ProductResponse> UpdateProduct(Guid id, UpdateProductRequest request);
        Task<Pagination<ProductsResponse>> GetProducts(GetProductRequest request);
        Task<ProductResponseDetail> GetProductDetail(Guid productId);

        Task<bool> DeleteProduct(Guid productId);

        Task AddRatingProduct(Guid id,ProductRatingRequest request);

        Task ActiveProdudct(Guid productId);

        Task InactiveProduct(Guid productId);
    }
}