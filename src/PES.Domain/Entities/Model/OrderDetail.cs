using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model
{
    public class OrderDetail  : BaseAuditableEntity
    {
        public int Price { get; set; }  
        
        public Product? Product  { get; set; }
        
        public Guid ProductId  { get; set; }

        public  Order? Order{ get; set; } = null;
        
        public Guid OrderId { get; set; }
    }
}