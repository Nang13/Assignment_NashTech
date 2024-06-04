using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.CategoryDTO
{

   public class AddNewCategoryRequest : IValidatableObject
   {
      [Required]
      [Length(5, 20, ErrorMessage = "Length name in limit (5,20)")]
      public string? CategoryName { get; set; }
      [Required]
      [Length(10, 50, ErrorMessage = "Length description in limit (10,50)")]
      public string? CategoryDescription { get; set; }
      [AllowNull]
      public string? CategoryMain { get; set; } = null!;
      [AllowNull]
      public Guid? CategoryParentId { get; set; } = null!;
      [JsonIgnore]
      public int AddType { get; set; } = 0;

      public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
      {
         //? 
         if (CategoryParentId is null)
         {
            //? Create ParentCategory so the CategoryMain is not null
            if (CategoryMain is null)
            {
               yield return new ValidationResult("To Set ParentCategory is CategoryMain is not null");
            }
            else
            {
               //? 1. Add CategoryParent
               AddType = 1;
            }
         }
         else
         {
            //? 1. Add CategoryChiled
            AddType = 2;
         }
      }
   };

   public class UpdateCategoryRequest : IValidatableObject
   {
      public string? CategoryName { get; set; } = null!;
      public string? CategoryDescription { get; set; } = null!;

      [JsonIgnore]
      public Dictionary<string, object?>? ObjectUpdate { get; set; } = [];

      public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
      {
         if (CategoryName is null && CategoryDescription is null)
         {
            yield return new ValidationResult("Nothing to update");
         }


         if (CategoryName is not null)
         {
            ObjectUpdate.Add(nameof(CategoryName), CategoryName);
         }

         if (CategoryDescription is not null)
         {
            ObjectUpdate.Add(nameof(CategoryDescription), CategoryDescription);
         }

      }
   };


}