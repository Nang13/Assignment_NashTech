using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PES.Application.IService;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.Domain.Tests;
using PES.Infrastructure.Common;
using PES.Presentation.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Presentation.Tests.Controller
{
    public class ProductControllerTest : SetUpTest
    {
        private readonly ProductController _controller;
        public ProductControllerTest()
        {
            _controller = new ProductController(_productServiceMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var mock = _fixture.Build<AddNewProductRequest>().Without(x => x.ProductName).Create();
            // Act
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Add(mock);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenModelStateIsValid()
        {
            // Arrange
            var mock = _fixture.Build<AddNewProductRequest>().Create();
            var mockResponse = _fixture.Build<ProductResponse>().Create();
            _productServiceMock.Setup(service => service.AddNewProduct(It.IsAny<AddNewProductRequest>())).ReturnsAsync(mockResponse);



            // Act
            var result = await _controller.Add(mock);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Create Product Successfully", okResult.Value);
            _productServiceMock.Verify(service => service.AddNewProduct(mock), Times.Once);
        }


        [Fact]
        public async Task Patch_ShouldReturnOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mock = _fixture.Build<UpdateProductRequest>().Create();
            var mockResponse = _fixture.Build<ProductResponse>().Create();

            var productId = Guid.NewGuid();


            _productServiceMock.Setup(service => service.UpdateProduct(productId, mock))
                              .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Patch(productId, mock);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Update Product Successfully", okResult.Value);
            _productServiceMock.Verify(service => service.UpdateProduct(productId, mock), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WithListOfProducts()
        {

            var request = _fixture.Build<GetProductRequest>().Create();

            var response = _fixture.Build<Pagination<ProductsResponse>>().Create();

            _productServiceMock.Setup(service => service.GetProducts(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.Get(request.Filter, request.PageNumber, request.PageSize);

            // Assert
            // Verify that the response is of type OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);


            // Verify that the GetProducts method was called once with the correct parameters
            _productServiceMock.Verify(service => service.GetProducts(It.Is<GetProductRequest>(r =>
                r.Filter == request.Filter && r.PageNumber == request.PageNumber && r.PageSize == request.PageSize)), Times.Once);



        }


        [Fact]
        public async Task GetById_ShouldRetunOk_WithProductDetail()
        {
          
            var response = _fixture.Build<ProductResponseDetail>().Create();

             _productServiceMock.Setup(x => x.GetProductDetail(response.Id)).ReturnsAsync(response);

            var result = await _controller.GetDetailById(response.Id);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(response, okResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnOk_WithMessage()
        {
            var request = Guid.NewGuid();
            var response = _fixture.Build<ProductResponseDetail>().With(x => x.Id, request).Create();

            _productServiceMock.Setup(x => x.DeleteProduct(request)).ReturnsAsync(true);

            var result = await _controller.Delete(request);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Delete Successfully", okObjectResult.Value);
        }

        [Fact]
        public async Task RateProduct_ShouldReturnOk_WithMessage()
        {
            var producIdMock = Guid.NewGuid();
            var requesMock = _fixture.Build<ProductRatingRequest>().Create();
            _productServiceMock.Setup(x => x.AddRatingProduct(producIdMock, requesMock));

            var result = await _controller.RateProduct(producIdMock, requesMock);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Rating Successfully", okObjectResult.Value);
        }

      
    }
}
