using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PES.Domain.DTOs.ProductDTO
{
    public class AddNewProductRequest
    {

        [Required]
        public string? ProductName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        public string? Description { get; set; } = null;
        public ImportantImformationRequest? ImformationRequest { get; set; } = null;
        public NutrionInforrmationRequest? NutrionInforrmationRequest { get; set; } = null;

        public List<string> ListImages { get; set; } = [];

        [JsonIgnore]
        public string? MainImage { get; set; }


        //? make the list be unique
        public Guid CategoryId { get; set; }

    }
    public class ImportantImformationRequest
    {
        public string? Ingredients { get; set; } = null;
        public string? Directions { get; set; } = null!;
        public string? LegalDisclaimer { get; set; } = null!;
    }

    public class NutrionInforrmationRequest
    {
        public decimal Calories { get; set; } = 0;
        public decimal Protein { get; set; } = 0;
        public decimal Sodium { get; set; } = 0;

        public decimal Fiber { get; set; } = 0;
        public decimal Sugars { get; set; } = 0;
    }
    public class UpdateProductRequest : IValidatableObject
    {
        public string? ProductName { get; set; } = null!;
        public decimal? Price { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public int? Quantity { get; set; } = null!;
        public Guid? CategoryId { get; set; } = null!;
        public NutrionInforrmationRequest? nutrionInfo { get; set; } = null!;

        public ImportantImformationRequest? importantInfo { get; set; } = null!;
        public List<string> productImages { get; set; } = [];

        [JsonIgnore]
        public Dictionary<string, object?>? ObjectUpdate { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var propertiesToUpdate = new Dictionary<string, object>
{
    { nameof(ProductName), ProductName },
    { nameof(Price), Price },
    { nameof(Description), Description },
    { nameof(Quantity), Quantity },
    { nameof(CategoryId), CategoryId }
};

            foreach (var property in propertiesToUpdate)
            {
                if (property.Value != null)
                {
                    ObjectUpdate.Add(property.Key, property.Value);
                }
            }

            if (ObjectUpdate.Count == 0)
            {
                if (nutrionInfo is null && importantInfo is null && productImages is null)
                {
                    yield return new ValidationResult("Nothing to update");
                }
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