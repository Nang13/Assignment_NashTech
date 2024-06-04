using PES.Domain.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.OrderDTO
{
    public static class MapperOrderBuilder
    {
        public static Order MapperDTO(this OrderRequest request, string userID,Guid OrderID)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));
            return new Order
            {
                Id = OrderID,
                UserId = userID,
                CreatedBy = userID,
                Created = DateTime.UtcNow.AddHours(7),
                TotalPrice = request.Total
                
            };
        }


        public static OrderDetail MapperDTO(this OrderDetailRequest request,Guid OrderID)
        {
            return request == null
                ? throw new ArgumentNullException(nameof(request))
                : new OrderDetail
            {
                Created = DateTime.UtcNow.AddHours(7),
                OrderId = OrderID,
                Price = request.Price,
                ProductId = request.ProductId,
                TotalPrice = request.Price * request.Quantity,
                Quantity = request.Quantity,
            };
        }

        public static OrderResponse MapperDTO(this Order order)
        {
            return order == null
               ? throw new ArgumentNullException(nameof(order))
               : new OrderResponse(OrderId: order.Id, TotalPrice: order.TotalPrice, ProductCount: order.OrderDetails.Count);


        }
    }
}
