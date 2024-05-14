using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PES.Domain.Constant
{
    public static class CurrentTime
    {
        public static readonly DateTime RecentTime = DateTime.UtcNow.AddHours(7);
    }
}