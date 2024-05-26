using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.Product
{
    public record ProductResponse(Guid Id, string ProductName, DateTime CreatedDate);
    public record ProductResponseDetail(Guid Id, string ProductName, decimal Price, NutrionInfo? NutrionInfo , ProductCategory? ProductCategory, ImportantInfo? ImportantInfo, List<ProductImageResponse> ProductImages );

    public record ProductCategory(Guid categoryId, string CategoryName, string CategoryMain);
    public record ProductImageResponse(string url ,bool isMain);
    public record NutrionInfo(decimal? Calories, decimal? Protein, decimal? Sodium, decimal? Fiber, decimal? Sugars);
    public record ImportantInfo(string Ingredients, string Directions, string LegalDisclaimer);
    public record ProductsResponse(Guid Id, string ProductName, DateTime CreatedDate, string ImageMain, string CategoryMain, string CategoryName, decimal Price, string Description);
    //Include(x => x.Category).Include(x => x.ProductImages)
}