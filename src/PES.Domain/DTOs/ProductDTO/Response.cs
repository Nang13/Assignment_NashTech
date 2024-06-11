using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.ProductDTO
{
    public record ProductResponse(Guid Id, string ProductName, DateTime CreatedDate);
    public record ProductResponseDetail(Guid Id, string ProductName, int Quantity,decimal Price, string Status, bool? IsDeleted, string ProductDescription,NutrionInfo? NutrionInfo, ProductCategory? ProductCategory, ImportantInfo? ImportantInfo, List<ProductImageResponse> ProductImages, IReadOnlyCollection<RatingResponse> Ratings, DateTime CreatedDate, DateTime LastModifiedDate);


     public  record RatingResponse(string UserId,string UserComment,int UserRating ,string UserName,DateTime commentDate);
    public record ProductCategory(Guid categoryId, string CategoryName, string CategoryMain);
    public record ProductImageResponse(string url ,bool isMain);
    public record NutrionInfo(decimal? Calories, decimal? Protein, decimal? Sodium, decimal? Fiber, decimal? Sugars);
    public record ImportantInfo(string Ingredients, string Directions, string LegalDisclaimer);
    public record ProductsResponse(Guid Id, string ProductName, double Rating,DateTime CreatedDate, string ImageMain, string CategoryMain, string CategoryName, decimal Price, string Description, Guid CategoryId,string Status,bool isDeleted);
    //Include(x => x.Category).Include(x => x.ProductImages)
}