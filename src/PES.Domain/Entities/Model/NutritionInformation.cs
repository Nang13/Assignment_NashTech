using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model
{
    public class NutritionInformation : BaseAuditableEntity
    {


        public decimal Calories { get; set; } = 0;
        public decimal Protein { get; set; } = 0;
        public decimal Sodium { get; set; } = 0;

        public decimal Fiber { get; set; } = 0;
        public decimal Sugars { get; set; } = 0;

        public Product? Product { get; set; } = null;

        public Guid ProductId { get; set; }

    }
}
