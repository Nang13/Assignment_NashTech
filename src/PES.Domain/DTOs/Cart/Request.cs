using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace PES.Domain.DTOs.Cart
{
    public class Request
    {

    }


    public class AddProductToCartRequest
    {
        public Guid ProductId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        [Required]
        [Range(0, 2)]
        public int CartActionType { get; set; }
    }
    public class CartModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class Cart
    {
        public decimal TotalPrice { get; set; }
        public List<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
        public decimal TotalPrice {get ;set;}
    }
}