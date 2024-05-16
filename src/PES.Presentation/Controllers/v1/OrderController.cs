using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.Order;

namespace PES.Presentation.Controllers.v1
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


        [HttpGet]
        public async Task<ActionResult> GetOrder()
        {
            var response = await _orderService.GetOrder();
            return Ok(response);
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetail(Guid orderId) { 
            var response = await _orderService.GetOrderDetail(orderId);
            return Ok(response); 
        }


    }
}