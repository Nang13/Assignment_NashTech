using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PES.Application.Helper.ErrorHandler;
using PES.Application.Helper.RedisHandler;
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

            string user =  _claimsService.GetCurrentUserId;
            var productId = Guid.Parse(cartItems.ProductId.ToString());
            var productKey = productId.ToString();
            Task.Run(() => AddProductToCartHandlers(user, productId));
            switch (cartItems.CartActionType)
            {
                case 0:
                    await AddOrUpdateProductInCartAsync(productId, productKey, user, cartItems.Quantity);
                    break;
                case 1:
                    await IncrementProductQuantityAsync(productId, productKey, user, cartItems.Quantity);
                    break;
                case 2:
                    await DecrementProductQuantityAsync(productId, productKey, user, cartItems.Quantity);
                    break;
                default:
                    await RemoveProductFromCartAsync(productKey, user);
                    break;
            }
        }

        public async ValueTask AddProductToCartHandlers(string UserId, Guid ProductId)
        {
            using var objectT = new PopularProductHandler(_database);
            await objectT.ProductVoting(UserId, ProductId, 2);
        }
        private async Task AddOrUpdateProductInCartAsync(Guid productId, string productKey, string user, int quantity)
        {
            if (await CheckInCartBefore(productId, user))
            {
                await _database.HashIncrementAsync(user, productKey, quantity);
            }
            else
            {
                _database.HashSet(user, productKey, quantity);
            }
        }

        private async Task IncrementProductQuantityAsync(Guid productId, string productKey, string user, int quantity)
        {
            var quantityInCart = (int)await _database.HashGetAsync(user, productKey);

            if (await CheckInputIsSensitive(productId, quantityInCart, quantity))
            {
                await _database.HashIncrementAsync(user, productKey, quantity);
            }
            else
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Cannot add more than Product Quantity");
            }
        }

        private async Task DecrementProductQuantityAsync(Guid productId, string productKey, string user, int quantity)
        {
            await _database.HashDecrementAsync(user, productKey, quantity);
            await CheckAndDeleteOutCart(productId, user);
        }

        private async Task RemoveProductFromCartAsync(string productKey, string user)
        {
            await _database.HashDeleteAsync(user, productKey);
        }
        private async ValueTask CheckAndDeleteOutCart(Guid productId, string userId)
        {
            int productQuantityCart = (int)await _database.HashGetAsync(userId, productId.ToString());
            if (productQuantityCart == 0) await _database.HashDeleteAsync(userId, productId.ToString());
        }

        private async ValueTask<bool> CheckInCartBefore(Guid productId, string userId)
        {
            if (_database.HashGetAsync(userId, productId.ToString()) is null)
            {
                return false;
            }
            else { return true; }
        }

        private async ValueTask<bool> CheckInputIsSensitive(Guid productId, int quantityProduct, int quantityInput)
        {
            int ProductQuantity = _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == productId).Result.Quantity;
            if (ProductQuantity < quantityInput + quantityProduct)
            {
                return false;
            }
            return true;

        }

        public async Task<Cart> GetCart()
        {
            string userId = _claimsService.GetCurrentUserId;
            decimal TotalPrice = 0;
            List<CartItem> carts = [];
            HashEntry[] dataChecker = _database.HashGetAll(userId);
            foreach (HashEntry dataCheckerItem in dataChecker)
            {
                Product product = await _unitOfWork.ProductRepository.GetByIdAsync(Guid.Parse(dataCheckerItem.Name));
                ProductImage productImage = await _unitOfWork.ProductImageRepository.FirstOrDefaultAsync(x => x.ProductId == Guid.Parse(dataCheckerItem.Name) && x.IsMain == true);
                CartItem cartItem = new()
                {
                    Id = Guid.Parse(dataCheckerItem.Name),
                    Price = product.Price,
                    Quantity = int.Parse(dataCheckerItem.Value),
                    Name = product.ProductName,
                    TotalPrice = product.Price * int.Parse(dataCheckerItem.Value),
                    ProductImage = productImage.ImageUrl,
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

       

     

        
    }
}