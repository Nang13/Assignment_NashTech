using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.Cart
{
    public class Request
    {

    }


    public class Cart
    {
        public int TotalPrice { get; set; } 
        public List<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}