using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PES.Domain.Entities.Common;
using PES.Infrastructure.Common;

namespace PES.Infrastructure.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseAuditableEntity
    {
        Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes);
        Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> AddAsync(TEntity entity);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        Task AddRangeAsync(List<TEntity> entities);
        Task<Pagination<TEntity>> ToPagination(int pageNumber = 0, int pageSize = 10);
    }
}