using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Application.IService
{
     public interface IClaimsService
    {
        public string GetCurrentUserId { get; }

        public string UserRole { get; }
    }

}