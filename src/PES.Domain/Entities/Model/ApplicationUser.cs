using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace PES.Domain.Entities.Model
{
    public class ApplicationUser  : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;   
        public string LastName { get; set; } = string.Empty ;
        public string Address { get; set; } = string.Empty;
        public virtual ICollection<Order>? Orders{ get; set; } = [];
        public virtual ICollection<ProductRating>? ProductRatings{ get; set; } = [];

    }
}