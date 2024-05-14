using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.DTOs.Category
{
   
   public record AddNewCategoryRequest(string CategoryName,string CategoryMain,Guid CategoryParentId);

   public record UpdateCategoryRequest(string CategoryName);


}