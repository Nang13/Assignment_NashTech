using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.Product
{
    public record ProductResponse(Guid Id, string ProductName, DateTime CreatedDate);
    public record ProductResponseDetail(Guid Id, string ProductName, decimal Price);
}