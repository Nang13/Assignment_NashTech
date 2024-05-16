using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PES.Domain.DTOs.Product
{
    public class AddNewProductRequest
    {

        [Required]
        public string? ProductName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }


        public List<IFormFile> ListImages { get; set; } = [];

        public IFormFile? MainImage { get; set; }


        //? make the list be unique
        //public List<Guid> CategoryId { get; set; }

    }


    public class UpdateProductRequest : IValidatableObject
    {
        public string? ProductName { get; set; } = null!;

        public decimal? Price { get; set; } = null!;

        [JsonIgnore]
        public Dictionary<string, object?>? ObjectUpdate { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProductName is not null)
            {
                ObjectUpdate.Add(nameof(ProductName), ProductName);
            }
            if (Price is not null)
            {
                ObjectUpdate.Add(nameof(Price), Price);
            }


            if (ProductName is null && Price is null)
            {

                yield return new ValidationResult("Nothing to update");
            }

        }
    }


    public class GetProductRequest
    {
        public Dictionary<string, string>? Filter { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}