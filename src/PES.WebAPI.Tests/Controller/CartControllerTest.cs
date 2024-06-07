using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PES.Domain.DTOs.Cart;
using PES.Domain.Tests;
using PES.Presentation.Controllers.v1;

namespace PES.WebAPI.Tests.Controller
{
    public class CartControllerTest : SetUpTest
    {

        private readonly CartController _cartController;

        public CartControllerTest()
        {
            _cartController = new CartController(_cartServiceMock.Object);
        }


        [Fact]
        public async Task AddCart_WithValidRequest_ReturnsOkResult()
        {
            var mock = _fixture.Build<AddProductToCartRequest>().Create();
            _cartServiceMock.Setup(x => x.AddProductToCart(mock));

            var result = await _cartController.AddCart(mock);


            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Add Cart Successfully", okResult.Value);

            _cartServiceMock.Verify(x => x.AddProductToCart(mock), Times.Once);


        }

        [Fact]
        public async Task GetCart_ReturnsOkResult()
        {
            var mock = _fixture.Build<Cart>().Create();

            _cartServiceMock.Setup(x => x.GetCart());

            var result = await _cartController.GetCart();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualCartItems = Assert.IsType<Cart>(okObjectResult.Value);

            Assert.Equal(mock, actualCartItems);
            _cartServiceMock.Verify(x => x.GetCart(), Times.Once);
        }
    }
}