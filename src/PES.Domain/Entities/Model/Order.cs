using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model
{
    public class Order : BaseAuditableEntity 
    {
        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails{ get; set; } = new List<OrderDetail>();

        public ApplicationUser? User { get; set; } = null;
      
        public string? UserId { get; set; }
        
    }
}