using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.DTOs.ProductDTO;

namespace PES.Presentation.Controllers.V1
{
    public class OrderController : DefaultController
    {
        /*
        todo create order
        todo get orders
        todo get order detail
        */

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderRequest request)
        {
            var response = await _orderService.AddOrder(request);
            return Ok(response);
        }


        [HttpGet("user/{UserId}")]
        public async Task<IActionResult> GetOrderByUserId(string UserId)
        {
            var response = await _orderService.GetOrderByUser(UserId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrder([FromQuery] Dictionary<string, string> filter, [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 1)
        {
            var response = await _orderService.GetOrder(new GetOrderRequest { Filter = filter, PageNumber = pageNumber, PageSize = pageSize });
            return Ok(response);
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetail(Guid orderId) { 
            var response = await _orderService.GetOrderDetail(orderId);
            return Ok(response); 
        }


    }
}