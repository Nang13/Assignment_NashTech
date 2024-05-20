using PES.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Infrastructure.Repository
{
    public class ImportantRepository :  GenericRepository<ImportantInformation>,IImportantInfoRepository 
    {
        public ImportantRepository(PlantManagementContext context) : base(context)
        {
            
        }
    }
}
