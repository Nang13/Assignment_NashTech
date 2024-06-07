using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.Entities.Model;
using PES.Infrastructure.Common;

namespace PES.Application.IService
{
    public interface IOrderService
    {
        public Task<OrderResponse> AddOrder(OrderRequest request);
        public Task<Pagination<OrderResponse>> GetOrder(GetOrderRequest request);
        public Task<OrderSingleResponse> GetOrderDetail(Guid id);

        public Task<FrozenSet<OrderResponse>> GetOrderByUser(string UserId);

        public Task<bool> SetFinishOrder(Guid OrderId);
    }
}