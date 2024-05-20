using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace PES.Domain.Entities.Model
{
    public class Category : BaseAuditableEntity
    {
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }

        public int CategoryLeft { get; set; }
        public int CategoryRight { get; set; }
        public string? CategoryMain { get; set; }

        public Guid CategoryParentId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; } = [];



    }
}