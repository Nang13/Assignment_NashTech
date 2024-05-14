using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.Category
{
    public record CategoryDetailResponse(Guid CategoryId, string CategoryName, string CategoryMain, int Left, int Right);

    public class CategoryResponse
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }



}