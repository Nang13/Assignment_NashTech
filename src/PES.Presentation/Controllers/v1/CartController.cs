using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;
using PES.Domain.DTOs.Cart;
using PES.Presentation.Controllers.V1;

namespace PES.Presentation.Controllers.v1
{
    public class CartController : DefaultController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCart(List<CartItem> cartItems)
        {
            await _cartService.AddProductToCart(cartItems);
            return Ok("Add Cart Successfully");
        }
    }
}