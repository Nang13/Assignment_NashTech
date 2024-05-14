using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Common
{
    public  abstract class BaseEntity
    {
         public Guid Id { get; set; }
    }
}