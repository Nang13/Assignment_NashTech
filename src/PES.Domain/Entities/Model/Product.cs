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
        public Category Category { get; set; } = null;
        public string? Description { get; set; }
        
        public Guid CategoryId { get; set; }

        public virtual ICollection<ProductImage> ProductImages{ get; set; } = new List<ProductImage>();
        public ProductRating? ProductRating { get; set; } 
        public virtual ICollection<OrderDetail>? OrderDetails { get; set;} = [];
        public NutritionInformation? NutritionInformation { get; set; }
        public ImportantInformation? ImportantInformation { get; set; }

    }
}