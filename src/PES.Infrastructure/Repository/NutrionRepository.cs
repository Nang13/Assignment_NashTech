using PES.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Infrastructure.Repository
{
    public class NutrionRepository : GenericRepository<NutritionInformation> ,INutrionInfoRepository
    {
        public NutrionRepository(PlantManagementContext context) : base(context)
        {
                
        }
    }
}
