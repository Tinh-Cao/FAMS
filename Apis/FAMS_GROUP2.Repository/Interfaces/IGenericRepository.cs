using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> UpdateRange(List<TEntity> entities);
        Task<bool> SoftRemove(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task<bool> SoftRemoveRange(List<TEntity> entities);

        Task<Pagination<TEntity>> ToPagination(PaginationParameter paginationParameter);
    }
}
