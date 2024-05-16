using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace PES.Domain.Entities.Model
{
    public class ProductRating : BaseAuditableEntity
    {
        public ApplicationUser? User { get; set; }
        public string? UserId { get; set; }

        //? 1-5
        public int Rating { get; set; } 
        public string Comment { get; set; } = string.Empty;

        public int UsefulComment {get; set; }
        public Product? Product{ get; set; }
        public Guid ProductId { get; set; }

    }
}