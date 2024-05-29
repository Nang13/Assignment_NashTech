using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PES.Application.IService;
using PES.Domain.DTOs.Order;
using PES.Domain.Entities.Model;
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

        public OrderService(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
        }
        public async Task<OrderResponse> AddOrder(OrderRequest request)
        {
            string userId  = "a3cf5c94-b866-4b80-982a-243daa7edf87";
            Guid orderId = Guid.NewGuid();
            Order order = new()
            {
                Id = orderId,
                CreatedBy = userId,
                UserId = userId,
                TotalPrice = request.Total,

            };
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangeAsync();
            await AddOrderDetail(request.orderDetails, orderId);

            return new OrderResponse(OrderId: orderId, TotalPrice: request.Total);
        }
        private async Task AddOrderDetail(List<OrderDetailRequest> request, Guid OrderId)
        {
            await _unitOfWork.OrderDetailRepository.AddRangeAsync(request.Select(order => new OrderDetail
            {
                OrderId = OrderId,
                Price = order.Price,
                ProductId = order.ProductId,
                TotalPrice = order.Price * order.Quantity
            }).ToList());
            await _unitOfWork.SaveChangeAsync();
        }


        public async Task<OrderSingleResponse> GetOrderDetail(Guid id)
        {
            string userId = "a3cf5c94-b866-4b80-982a-243daa7edf87f";
            Order order = await _unitOfWork.OrderRepository.GetByIdAsync(id) ?? throw new Exception("hihi");
            var orderDetail = _unitOfWork.OrderDetailRepository.WhereAsync(x => x.OrderId == id).Result.Select(x => new OrdererDetailResponse(OrderDetailId: x.Id, Price: x.Price)).ToList();
            return new OrderSingleResponse(OrderId: order.Id, TotalPrice: order.TotalPrice, OrdererDetails: orderDetail);

        }

        public async Task<IReadOnlyList<OrderResponse>> GetOrder()
        {
            string userId = "a3cf5c94-b866-4b80-982a-243daa7edf87";
            var order = _unitOfWork.OrderRepository.WhereAsync(x => x.UserId == userId).Result.Select(x => new OrderResponse(OrderId: x.Id, TotalPrice: x.TotalPrice)).ToList();
            return order;
        }
    }
}