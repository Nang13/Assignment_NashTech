using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Domain.DTOs.Order;
using PES.Domain.Entities.Model;

namespace PES.Application.IService
{
    public interface IOrderService
    {
        public Task<OrderResponse> AddOrder(OrderRequest request);
        public Task<IReadOnlyList<OrderResponse>> GetOrder();
        public Task<OrderSingleResponse> GetOrderDetail(Guid id);
    }
}