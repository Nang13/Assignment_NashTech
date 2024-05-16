using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Common
{
   public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
}