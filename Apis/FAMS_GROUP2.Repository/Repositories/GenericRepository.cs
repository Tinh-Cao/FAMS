using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly FamsDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public GenericRepository(FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService)
        {
            _dbSet = context.Set<TEntity>();
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }


        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedDate = _timeService.GetCurrentTime();
            entity.CreatedBy = _claimsService.GetCurrentUserId.ToString();
            await _dbSet.AddAsync(entity);
            //await _dbContext.SaveChangesAsync();
            return entity;
        }

        // riêng hàm này đã sửa để adapt theo Unit Of Work
        public async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedDate = _timeService.GetCurrentTime();
                entity.CreatedBy = _claimsService.GetCurrentUserId.ToString();
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<bool> SoftRemove(TEntity entity)
        {
            entity.IsDelete = true;
            entity.DeletedDate = _timeService.GetCurrentTime();
            entity.DeletedBy = _claimsService.GetCurrentUserId.ToString();
            _dbSet.Update(entity);
           // await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftRemoveRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDelete = true;
                entity.DeletedDate = _timeService.GetCurrentTime();
                entity.DeletedBy = _claimsService.GetCurrentUserId.ToString();
            }
            _dbSet.UpdateRange(entities);
          //  await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Pagination<TEntity>> ToPagination(PaginationParameter paginationParameter)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<TEntity>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<bool> Update(TEntity entity)
        {
            entity.ModifiedDate = _timeService.GetCurrentTime();
            entity.ModifiedBy = _claimsService.GetCurrentUserId.ToString();
            _dbSet.Update(entity);
         //   await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedDate = _timeService.GetCurrentTime();
                entity.CreatedBy = _claimsService.GetCurrentUserId.ToString();
            }
            _dbSet.UpdateRange(entities);
          //  await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
