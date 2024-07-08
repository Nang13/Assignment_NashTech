using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Infrastructure.Data.Config
{
    public class MessagesStorageDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string MessagesCollectionName { get; set; } = null!;
    }
}
