using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.Order
{


    public class ProductRatingRequest
    {
        [Required]
        [Range(1, 5)]
        public int? Rating { get; set; }

        [Required]
        [Length(5, 50)]
        public string? Comment { get; set; }
    }


    public class OrderRequest : IValidatableObject
    {
        public List<OrderDetailRequest> orderDetails { get; set; } = [];

        [JsonIgnore]
        public decimal Total { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Total += orderDetails.Select(x => x.Price * x.Quantity).Sum();
            if (orderDetails is null)
            {
                return Enumerable.Empty<ValidationResult>();
            }
            return Enumerable.Empty<ValidationResult>();
        }
    }

    public class OrderDetailRequest
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Guid ProductId { get; set; }
    }
}