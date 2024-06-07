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
        public static Order MapperDTO(this OrderRequest request, string userID, Guid OrderID)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return new Order
            {
                Id = OrderID,
                UserId = userID,
                CreatedBy = userID,
                Created = DateTime.UtcNow.AddHours(7),
                TotalPrice = request.Total

            };
        }


        public static OrderDetail MapperDTO(this OrderDetailRequest request, Guid OrderID,string userID)
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
                    CreatedBy =  userID
                };
        }

        public static OrderResponse MapperDTO(this Order order, string UserID, string UserName)
        {
            return order == null
               ? throw new ArgumentNullException(nameof(order))
               : new OrderResponse(OrderId: order.Id, TotalPrice: order.TotalPrice, ProductCount: order.OrderDetails.Count, Status: order.Status, PaymentType: order.PaymentType, OrderCurrencyCode: order.CurrencyCode, UserID: UserID
               , UserName);


        }


        public static OrdererDetailResponse MapDTO(this OrderDetail orderDetail)
        {
            return orderDetail == null
          ? throw new ArgumentNullException(nameof(orderDetail)) :
             new OrdererDetailResponse(OrderDetailId: orderDetail.Id, Price: orderDetail.Price, ProductName: orderDetail.Product.ProductName, ProductImage: orderDetail.Product.ProductImages.FirstOrDefault(pro => pro.IsMain == true).ImageUrl, Quantity: orderDetail.Quantity, TotalPrice: orderDetail.TotalPrice);
        }
    }
}
