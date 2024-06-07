using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.Tests;
using PES.Infrastructure.Common;
using PES.Presentation.Controllers.V1;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Presentation.Tests.Controller
{
    public class OrderControllerTest : SetUpTest
    {
        private readonly OrderController _orderController;
        public OrderControllerTest()
        {
            _orderController = new OrderController(_orderServiceMock.Object);
        }

        [Fact]
        public async Task CreateOrder_ValidRequest_ReturnsOkResult()
        {
            var request = _fixture.Build<OrderRequest>().Create();
            var response = _fixture.Build<OrderResponse>().Create();
            _orderServiceMock.Setup(x => x.AddOrder(request)).ReturnsAsync(response);

            var result = await _orderController.CreateOrder(request);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualOrderResponse = Assert.IsType<OrderResponse>(okObjectResult.Value);


            Assert.Equal(response.UserName, actualOrderResponse.UserName);
            Assert.Equal(response.Status, actualOrderResponse.Status);
            Assert.Equal(response.TotalPrice, actualOrderResponse.TotalPrice);


        }
        [Fact]
        public async Task GetOrderByUserId_ValidUserId_ReturnsOkResult()
        {

            var mock = _fixture.Build<FrozenSet<OrderResponse>>().Create();
            var userId = _fixture.Build<string>().Create();
            _orderServiceMock.Setup(x => x.GetOrderByUser(userId)).ReturnsAsync(mock);

            var result = await _orderController.GetOrderByUserId(userId);
            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualOrders = Assert.IsType<List<OrderResponse>>(okObjectResult.Value);

            Assert.Equal(mock.Count, actualOrders.Count);
            _orderServiceMock.Verify(os => os.GetOrderByUser(userId), Times.Once);
        }


        [Fact]
        public async Task GetOrder_ReturnsOkResult()
        {
            var mock = _fixture.Build<GetOrderRequest>().Create();
            var response = _fixture.Build<Pagination<OrderResponse>>().Create();

            _orderServiceMock.Setup(x => x.GetOrder(mock)).ReturnsAsync(response);

            var result = await _orderController.GetOrder(mock.Filter, mock.PageNumber, mock.PageSize);


            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<Pagination<OrderResponse>>(okObjectResult.Value);

            Assert.Equal(response, actualResponse);
            _orderServiceMock.Verify(x => x.GetOrder(mock), Times.Once);
        }


        [Fact]
        public async Task GetOrderDetail_ReturnsOkResult()
        {
            var mock = _fixture.Build<OrderSingleResponse>().Create();
            _orderServiceMock.Setup(x => x.GetOrderDetail(mock.OrderId)).ReturnsAsync(mock);

            var result = await _orderController.GetOrderDetail(mock.OrderId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<OrderSingleResponse>(okObjectResult.Value);

            Assert.Equal(mock, actualResponse);
            _orderServiceMock.Verify(x => x.GetOrderDetail(mock.OrderId), Times.Once);

        }
    }
}
