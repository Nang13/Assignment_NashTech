using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PES.Application.IService;
using PES.Domain.DTOs.Cart;
using StackExchange.Redis;

namespace PES.Application.Service
{
    public class CartService : ICartService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IDatabase _database;
        private readonly IClaimsService _claimsService;

        public CartService(IDistributedCache distributedCache, IDatabase database, IClaimsService claimsService)
        {
            _distributedCache = distributedCache;
            _database = database;
            _claimsService = claimsService;
        }
        public async Task AddProductToCart(List<CartItem> cartItems)
        {
            //string items = JsonConvert.SerializeObject(cartItems);
           // _database.HashSet(_claimsService.GetCurrentUserId, "Name", cartItems[0].Name);
            //_database.HashSet(_claimsService.GetCurrentUserId,"Price",cartItems[0].Price.ToString());

            string cartKey = $"cart:{1}";
            string productKey = $"product:{1}";

            _database.HashSet(cartKey, productKey, 2);
        }

        public Task DecreaseQuantity(Guid ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> GetCart()
        {
            throw new NotImplementedException();
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