using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PES.Domain.Entities.Model
{
    public class ProductInCategory : BaseAuditableEntity
    {
        public Product? Product { get; set; } 
        public Guid ProductId { get; set; }


        public Category? Category { get; set; }

        public Guid CategoryId { get; set; }

    }
}