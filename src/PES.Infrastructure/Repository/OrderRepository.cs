using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PES.Infrastructure.Data;


namespace PES.Infrastructure.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(PlantManagementContext context) : base(context)  
        {
            
        }
    }
}