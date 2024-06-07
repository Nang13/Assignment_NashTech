using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PES.Application.Helper;
using PES.Application.Helper.RedisHandler;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.Domain.Enum;
using PES.Infrastructure.Common;
using PES.Infrastructure.UnitOfWork;
using StackExchange.Redis;
using Order = PES.Domain.Entities.Model.Order;
using Role = PES.Domain.Enum.Role;

namespace PES.Application.Service
{
    public class OrderService : IOrderService
    {
        /*
        todo: AddNewOrder
        todo: View AllOrder by UserId
        todo: View OrderDetail by UserId
        */

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDatabase _database;
        private const string LockKey = "order-processing-lock";
        private static readonly TimeSpan LockExpiry = TimeSpan.FromSeconds(30);

        public OrderService(IUnitOfWork unitOfWork, IClaimsService claimsService, UserManager<ApplicationUser> user, IDatabase database)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
            _userManager = user;
            _database = database;
        }
        public async Task<OrderResponse> AddOrder(OrderRequest request)
        {
            using (var redislock = new LockHandler(_database, LockKey, LockExpiry))
            {
                if (await redislock.AcquireLockAsync())
                {
                    try
                    {
                        string userId = _claimsService.GetCurrentUserId;
                        Guid orderId = Guid.NewGuid();
                        Order order = request.MapperDTO(userId, orderId);
                        var user = _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId).Result;

                        await _unitOfWork.OrderRepository.AddAsync(order);
                        await _unitOfWork.SaveChangeAsync();
                        await AddOrderDetail(request.orderDetails, orderId, userId);
                        return new OrderResponse(OrderId: orderId, TotalPrice: request.Total, ProductCount: 1, Status: order.Status, order.PaymentType, OrderCurrencyCode: order.CurrencyCode, UserID: userId, UserName: user.UserName);
                    }
                    finally
                    {
                        await redislock.ReleaseLockAsync();
                    }
                }
                else
                {
                    Console.WriteLine("Could not acquire lock to process order . Try again later.");
                }
            }

            return null;



        }
        private async Task AddOrderDetail(List<OrderDetailRequest> request, Guid OrderId, string UserID)
        {
            await _unitOfWork.OrderDetailRepository.AddRangeAsync(request.Select(order => order.MapperDTO(OrderId, UserID)).ToList());
            foreach (var item in request)
            {
                await _unitOfWork.ProductRepository.UpdateProduct(item.ProductId, item.Quantity);
            }
            await _unitOfWork.SaveChangeAsync();
        }


        public async Task<OrderSingleResponse> GetOrderDetail(Guid id)
        {
            //  string userId = _claimsService.GetCurrentUserId;
            Order order = await _unitOfWork.OrderRepository.GetByIdAsync(id) ?? throw new Exception("hihi");

            var orderDetail = _unitOfWork.OrderDetailRepository.WhereAsync(x => x.OrderId == id, x => x.Product, x => x.Product.ProductImages).Result.Select(x => x.MapDTO()).ToList();
            return new OrderSingleResponse(OrderId: order.Id, TotalPrice: order.TotalPrice, OrdererDetails: orderDetail);

        }

        public async Task<Pagination<OrderResponse>> GetOrder(GetOrderRequest request)
        {

            if (_claimsService.UserRole == Role.User)
            {
                string userId = _claimsService.GetCurrentUserId;
                var user = await _userManager.FindByIdAsync(userId);
                var order = _unitOfWork.OrderRepository.WhereAsync(x => x.UserId == userId, x => x.OrderDetails).Result.Select(x => x.MapperDTO(userId, user.UserName)).ToList();
                return new Pagination<OrderResponse>
                {
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItemsCount = order.Count(),
                    Items = PaginatedList<OrderResponse>.Create(
                  source: order.AsQueryable(),
                  pageIndex: request.PageNumber,
                  pageSize: request.PageSize)


                };
            }
            else
            {
                request.Filter!.Remove("pageSize");
                request.Filter!.Remove("pageNumber");
                var data = _unitOfWork.OrderRepository.GetAllAsync(x => x.User, x => x.OrderDetails).Result.Select(x => x.MapperDTO(x.User.Id, x.User.UserName)).ToList();
                var filterResult = request.Filter.Count > 0 ? new List<OrderResponse>() : data;
                if (request.Filter?.Count > 0)
                {
                    foreach (var filter in request.Filter)
                    {
                        filterResult = filterResult.Union(FilterUtilities.SelectItems(data, filter.Key, filter.Value)).ToList();
                    }
                }

                return new Pagination<OrderResponse>
                {
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItemsCount = filterResult.Count(),
                    Items = PaginatedList<OrderResponse>.Create(
                      source: filterResult.AsQueryable(),
                      pageIndex: request.PageNumber,
                      pageSize: request.PageSize)


                };
            }
        }

        public async Task<FrozenSet<OrderResponse>> GetOrderByUser(string UserId)
        {
            FrozenSet<OrderResponse> order = _unitOfWork.OrderRepository.WhereAsync(x => x.UserId == UserId, x => x.User).Result.Select(x => x.MapperDTO(x.User.Id, x.User.UserName)).ToFrozenSet();
            return order;
        }

        public async Task<bool> SetFinishOrder(Guid OrderId)
        {
            Order order = await _unitOfWork.OrderRepository.GetByIdAsync(OrderId, x => x.OrderDetails);
            if (order == null)
            {
                return false;
            }
            order.Status = OrderStatus.InFinish;
            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChangeAsync();
            Task.Run(() => AddProductToOrderHandlers(order.OrderDetails.ToList(), order.UserId));
            return true;
        }

        public async Task AddProductToOrderHandlers(List<OrderDetail> orders, string UserId)
        {
            using var redis = new PopularProductHandler(_database);
            foreach (var item in orders)
            {
                await redis.ProductVoting(UserId, item.ProductId, 2);

            }
        }
    }
}