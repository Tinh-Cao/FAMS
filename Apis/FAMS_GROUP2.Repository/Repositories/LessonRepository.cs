using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        private readonly FamsDbContext _dbContext;
        public LessonRepository(FamsDbContext context, 
            ICurrentTime timeService, 
            IClaimsService claimService) 
            : base(context, timeService, claimService)
        {
            _dbContext = context;
        }
        public async Task<Pagination<Lesson>> GetLessonByFilterAsync(PaginationParameter paginationParameter, LessonFilterModel lessonFilterModel)
        {
            try
            {
                var lessonsQuery = _dbContext.Lessons.AsQueryable();
                lessonsQuery = await ApplyFilterSortAndSearch(lessonsQuery, lessonFilterModel);
                if (lessonsQuery != null)
                {
                    var lessonQuery = ApplySorting(lessonsQuery, lessonFilterModel);
                    var totalCount = await lessonQuery.CountAsync();

                    var lessonPagination = await lessonQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToListAsync();
                    return new Pagination<Lesson>(lessonPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Lesson>> ApplyFilterSortAndSearch(IQueryable<Lesson> Query, LessonFilterModel lessonFilterModel)
        {
            if (lessonFilterModel == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(lessonFilterModel.Search))
            {
                Query = Query.Where(x => x.LessonCode.Contains(lessonFilterModel.Search));
            }
            if (lessonFilterModel.Status != null)
            {
                Query = Query.Where(x => x.Status == lessonFilterModel.Status);
            }
            if (lessonFilterModel.ModuleId != null)
            {
                Query = Query.Where(less => less.ModuleId == lessonFilterModel.ModuleId);
            }
            if (lessonFilterModel.isDelete == true)
            {
                Query = Query.Where(a => a.IsDelete == true);
            }
            else if (lessonFilterModel.isDelete == false)
            {
                Query = Query.Where(a => a.IsDelete == false);
            }
            else
            {
                Query = Query.Where(a => a.IsDelete == true || a.IsDelete == false);
            }
            return Query;
        }
        private IQueryable<Lesson> ApplySorting(IQueryable<Lesson> query, LessonFilterModel lessonFilterModel)
        {
            switch (lessonFilterModel.Sort.ToLower())
            { 
                case "lessonname":
                    query = (lessonFilterModel.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.LessonName) : query.OrderBy(x => x.LessonName);
                    break;
                default:
                    query = (lessonFilterModel.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }
            return query;
        }
    }
}
