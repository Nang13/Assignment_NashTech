using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.Cart;

namespace PES.Application.IService
{
    public interface ICartService
    {
        public Task AddProductToCart(AddProductToCartRequest cartItems);
        public Task IncreaseQuantity(Guid ProductId);
        public Task DecreaseQuantity(Guid ProductId);
        public Task<Cart> GetCart();



    }
}