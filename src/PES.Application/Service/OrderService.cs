using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PES.Application.IService;
using PES.Application.Utilities;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.Domain.Enum;
using PES.Infrastructure.Common;
using PES.Infrastructure.UnitOfWork;

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

        public OrderService(IUnitOfWork unitOfWork, IClaimsService claimsService, UserManager<ApplicationUser> user)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
            _userManager = user;
        }
        public async Task<OrderResponse> AddOrder(OrderRequest request)
        {
            string userId = _claimsService.GetCurrentUserId;
            Guid orderId = Guid.NewGuid();
            Order order = request.MapperDTO(userId, orderId);
            var user = _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId).Result;

            //Order order = new()
            //{
            //    Id = orderId,
            //    CreatedBy = userId,
            //    UserId = userId,
            //    TotalPrice = request.Total,

            //};
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangeAsync();
            await AddOrderDetail(request.orderDetails, orderId);

            return new OrderResponse(OrderId: orderId, TotalPrice: request.Total, ProductCount: 1, Status: order.Status, order.PaymentType, OrderCurrencyCode: order.CurrencyCode, UserID: userId, UserName: user.UserName);
        }
        private async Task AddOrderDetail(List<OrderDetailRequest> request, Guid OrderId)
        {
            //await _unitOfWork.OrderDetailRepository.AddRangeAsync(request.Select(order => new OrderDetail
            //{
            //    OrderId = OrderId,
            //    Price = order.Price,
            //    ProductId = order.ProductId,
            //    TotalPrice = order.Price * order.Quantity,
            //    Quantity = order.Quantity
            //}).ToList());
            await _unitOfWork.OrderDetailRepository.AddRangeAsync(request.Select(order => order.MapperDTO(OrderId)).ToList());
            await _unitOfWork.SaveChangeAsync();
        }


        public async Task<OrderSingleResponse> GetOrderDetail(Guid id)
        {
            //  string userId = _claimsService.GetCurrentUserId;
            Order order = await _unitOfWork.OrderRepository.GetByIdAsync(id) ?? throw new Exception("hihi");


            var orderDetail = _unitOfWork.OrderDetailRepository.WhereAsync(x => x.OrderId == id, x => x.Product, x => x.Product.ProductImages).Result.Select(x => new OrdererDetailResponse(OrderDetailId: x.Id, Price: x.Price, ProductName: x.Product.ProductName, ProductImage: x.Product.ProductImages.FirstOrDefault(pro => pro.IsMain == true).ImageUrl, Quantity: x.Quantity, TotalPrice: x.TotalPrice)).ToList();
            return new OrderSingleResponse(OrderId: order.Id, TotalPrice: order.TotalPrice, OrdererDetails: orderDetail);

        }

        public async Task<Pagination<OrderResponse>> GetOrder(GetOrderRequest request)
        {

            if (_claimsService.UserRole == Role.User)
            {
                string userId = _claimsService.GetCurrentUserId;
                var user = await _userManager.FindByIdAsync(userId);
                var order = _unitOfWork.OrderRepository.WhereAsync(x => x.UserId == userId, x => x.OrderDetails).Result.Select(x => x.MapperDTO(userId, user.UserName)).ToList();
                var dataCheck = new Pagination<OrderResponse>
                {
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItemsCount = order.Count(),
                    Items = PaginatedList<OrderResponse>.Create(
                  source: order.AsQueryable(),
                  pageIndex: request.PageNumber,
                  pageSize: request.PageSize)


                };
                // Serialize the result with reference handling


                return dataCheck;
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

                var dataCheck = new Pagination<OrderResponse>
                {
                    PageIndex = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItemsCount = filterResult.Count(),
                    Items = PaginatedList<OrderResponse>.Create(
                      source: filterResult.AsQueryable(),
                      pageIndex: request.PageNumber,
                      pageSize: request.PageSize)


                };
                // Serialize the result with reference handling


                return dataCheck;
            }


        }

        public async Task<FrozenSet<OrderResponse>> GetOrderByUser(string UserId)
        {
            FrozenSet<OrderResponse> order = _unitOfWork.OrderRepository.WhereAsync(x => x.UserId == UserId, x => x.User).Result.Select(x => x.MapperDTO(x.User.Id, x.User.UserName)).ToFrozenSet();
            return order;
        }
    }
}