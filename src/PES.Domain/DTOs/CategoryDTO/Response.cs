using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.CategoryDTO
{
    public record CategoryDetailResponse(Guid CategoryId, string CategoryName, string CategoryMain, int Left, int Right);

    public class CategoryResponse
    {
        public Guid CategoryId { get; set; }
        
        public string? CategoryName { get; set; }
        public int? Left { get; set; }

        public int? Right { get; set; }
        public Guid ParentId { get; set; }
        public string? CategoryDescription { get; set; }
    }


    



}