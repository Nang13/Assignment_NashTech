using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PES.Application.IService;

namespace PES.Presentation.Controllers.v1
{
    public class ProductController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // [HttpGet]
        // public Task<IActionResult> Get(){}
    }
}