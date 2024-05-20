using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.Helper;
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
        public async Task<IActionResult> AddCart(AddProductToCartRequest cartItems)
        {
            await _cartService.AddProductToCart(cartItems);
            return Ok("Add Cart Successfully");
        }


        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            //await _cartService.GetCart();
            return Ok(await _cartService.GetCart());
        }


        // [HttpPost("uploadImage")]
        // public async Task<IActionResult> UpImage([FromForm]AddIamge request )
        // {
        //     await StorageHandler.UploadFileAsync(request.file, request.file.Name);
        //     return Ok("com suon mon hoc");
        // }

        // public class AddIamge
        // {
        //     public IFormFile file { get; set; }
        // }
    }
}