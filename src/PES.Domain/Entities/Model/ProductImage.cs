using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PES.Domain.Entities.Model
{
    public class ProductImage : BaseAuditableEntity
    {

        //? 
        public string? ImageUrl { get; set; }

        public bool IsMain { get; set; }

        public Product? Product { get; set; }

        public Guid ProductId { get; set; }
    }
}