using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.DTOs.Cart;
using PES.Domain.Entities.Model;
using PES.Infrastructure.UnitOfWork;
using StackExchange.Redis;

namespace PES.Application.Service
{
    public class CartService : ICartService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IDatabase _database;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IDistributedCache distributedCache, IDatabase database, IClaimsService claimsService, IUnitOfWork unitOfWork)
        {
            _distributedCache = distributedCache;
            _database = database;
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
        }
        public async Task AddProductToCart(AddProductToCartRequest cartItems)
        {
            //string items = JsonConvert.SerializeObject(cartItems);
            // _database.HashSet(_claimsService.GetCurrentUserId, "Name", cartItems[0].Name);
            //_database.HashSet(_claimsService.GetCurrentUserId,"Price",cartItems[0].Price.ToString());
            string user = _claimsService.GetCurrentUserId;
            if (cartItems.CartActionType == 0)
            {
                _database.HashSet(user, cartItems.ProductId.ToString(), cartItems.Quantity);
            }
            else if (cartItems.CartActionType == 1)
            {
                await _database.HashIncrementAsync(user, cartItems.ProductId.ToString(), cartItems.Quantity);
            }
            else if (cartItems.CartActionType == 2)
            {

                await _database.HashDecrementAsync(user, cartItems.ProductId.ToString(), cartItems.Quantity);
            }
        }

        public Task DecreaseQuantity(Guid ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<Cart> GetCart()
        {
            string userId =_claimsService.GetCurrentUserId;
            decimal TotalPrice = 0;
            List<CartItem> carts = [];
            HashEntry[] dataChecker = _database.HashGetAll(userId);
            foreach (HashEntry dataCheckerItem in dataChecker)
            {
                Product product = await _unitOfWork.ProductRepository.GetByIdAsync(Guid.Parse(dataCheckerItem.Name));
                CartItem cartItem = new()
                {
                    Id = Guid.Parse(dataCheckerItem.Name),
                    Price = product.Price,
                    Quantity = int.Parse(dataCheckerItem.Value),
                    Name = product.ProductName,
                    TotalPrice = product.Price * int.Parse(dataCheckerItem.Value),
                    ProductImage = "banana.jpg",
                    IsSelected = false
                };
                TotalPrice += cartItem.Quantity * cartItem.Price;
                carts.Add(cartItem);

            }

            return new Cart
            {
                TotalPrice = TotalPrice,
                Items = carts

            };
        }

        public Task IncreaseQuantity(Guid ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> TestRedis()
        {
            SetCachedData<string>("bun dau mam tom", "hen xui", TimeSpan.FromHours(1));
            return "com suon";
        }

        public void SetCachedData<T>(string key, T data, TimeSpan cacheDuration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };
            var jsonData = System.Text.Json.JsonSerializer.Serialize(data);
            _distributedCache.SetString(key, jsonData, options);
        }
    }
}