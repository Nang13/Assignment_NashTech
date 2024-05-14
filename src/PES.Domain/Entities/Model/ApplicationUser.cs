using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace PES.Domain.Entities.Model
{
    public class ApplicationUser  : IdentityUser
    {
        public virtual ICollection<Order>? Orders{ get; set; } = [];
        public virtual ICollection<ProductRating>? ProductRatings{ get; set; } = [];

    }
}