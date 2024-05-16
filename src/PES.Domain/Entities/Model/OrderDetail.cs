using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model
{
    public class OrderDetail : BaseAuditableEntity
    {
        public decimal Price { get; set; } = 0;

        public Product? Product { get; set; }

        public decimal? TotalPrice { get; set; } = 0;
        public Guid ProductId { get; set; }

        public Order? Order { get; set; } = null;

        public Guid OrderId { get; set; }
    }
}