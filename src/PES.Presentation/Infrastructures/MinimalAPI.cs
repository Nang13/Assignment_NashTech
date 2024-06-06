using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PES.Infrastructure.Data;
using PES.Infrastructure.UnitOfWork;

namespace PES.Presentation.Infrastructures
{
    public static class MinimalAPI
    {
        public static void UseMinimalAPI(this WebApplication app)
        {


            //? using to check user is rating this product
            app.MapPost("/check/product/{productId}/user/{userId}", async (IUnitOfWork context, Guid productId, string userId) =>
             {

                 bool check = await context.ProductRepository.CheckProductRating(userId, productId);
                 if (check)
                 {
                     Results.Ok(new
                     {
                         message = "Not Rating Yet"
                     });

                 }
                 else
                 {
                     Results.BadRequest(new
                     {
                         message = " Rating Before"
                     });
                 }


             });

        }
    }
}