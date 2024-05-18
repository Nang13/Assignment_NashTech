using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model
{
    public class ImportantInformation : BaseAuditableEntity
    {
        public string? Ingredients { get; set; } = null;
        public string? Directions { get; set; } = null!;
        public string? LegalDisclaimer { get; set; } = null!;
        public Product? Product { get; set; } = null;
        public Guid ProductId { get; set; }

    }
}
