using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PES.Application.Helper;
using PES.Application.IService;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.DTOs.ProductDTO;

namespace PES.Presentation.Controllers.V1
{
    [ApiController]
    public class ProductController : DefaultController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // [HttpGet]
        // public Task<IActionResult> Get(){}



        /*
        ! not catch list image is unique all and if mainImage not have set first image in list is main
        ! check name is duplicate 
        ! add firebase to up real picture
        */
        [HttpPost]
        public async Task<IActionResult> Add(AddNewProductRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productService.AddNewProduct(request);
            return Ok("Create Product Successfully");
        }

        /*
        ? update price and name that be execute update implement
        ? can be null 1 
        ? check validation
        */

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, UpdateProductRequest request)
        {
            await _productService.UpdateProduct(id, request);

            return Ok("Update Product Successfully");
        }

        /*
        ! not test yet
        */
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Dictionary<string, string> filter, [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 10)
        {
            return Ok(await _productService.GetProducts(new GetProductRequest { Filter = filter, PageNumber = pageNumber, PageSize = pageSize }));
        }


        /*
        ! not check is in system
        */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailById(Guid id)
        {

            return Ok(await _productService.GetProductDetail(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            await _productService.DeleteProduct(id);
            return Ok("Delete Successfully");
        }

        [HttpPost("{productId}/rate")]
        public async Task<IActionResult> RateProduct(Guid productId, ProductRatingRequest request)
        {
            await _productService.AddRatingProduct(productId, request);
            return Ok("Rating Successfully");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile imageFile)
        {
           // await ImageHandlerExtension.UploadImageAsync(imageFile);
            await StorageHandler.UploadFileAsync(imageFile, "Product");
            return Ok(new
            {
                message = "Upload Image Successfully"
            });
        }


        [HttpPost("{productId}/active")]
        public async Task<IActionResult> ActiveProduct(Guid productId)
        {
            await _productService.ActiveProdudct(productId);
            return Ok(new
            {
                message = "Active Product Successfully"
            });
        }

        [HttpPost("{productId}/inactive")]
        public async Task<IActionResult> InactiveProduct(Guid productId)
        {
            await _productService.InactiveProduct(productId);
            return Ok(new
            {
                message = "InActive Product Successfully"
            });
        }
    }
}