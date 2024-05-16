using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model
{
    public class Product : BaseAuditableEntity
    {
        public string? ProductName { get; set; } 
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public string Status {get; set; } = "Active";
        public virtual ICollection<ProductInCategory> ProductInCategories{ get; set; } = new List<ProductInCategory>();
        public virtual ICollection<ProductImage> ProductImages{ get; set; } = new List<ProductImage>();
        public ProductRating? ProductRating { get; set; } 
        public virtual ICollection<OrderDetail>? OrderDetails { get; set;} = [];
    }
}