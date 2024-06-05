using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.OrderDTO
{
    public record OrderResponse(Guid OrderId, decimal TotalPrice,int ProductCount,string Status,string PaymentType,string? OrderCurrencyCode,string UserID,string UserName);
    
   
    public record OrderSingleResponse(Guid OrderId, decimal TotalPrice,IReadOnlyList<OrdererDetailResponse> OrdererDetails);


   
    public record OrdererDetailResponse(Guid OrderDetailId,decimal Price,string ProductName, string ProductImage,int? Quantity,decimal? TotalPrice);

}