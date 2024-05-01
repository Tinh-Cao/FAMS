using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FAMS_GROUP2.Repositories;

public class ClassRepository : GenericRepository<Class>, IClassRepository
{
    private readonly FamsDbContext _context;

    public ClassRepository(FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(
        context, timeService, claimsService)
    {
        _context = context;
    }

    public Task<Class> GetClassDetail(int classId)
    {
        var classObj = _context.Classes
            .Include(a => a.Program)
            .Include(a => a.ClassAccounts)
            .FirstOrDefaultAsync(a => a.Id == classId);
        return classObj;
    }

    public async Task<List<Class>> GetAllByAccountId(ClassesGetByAccountIdModel model)
    {
        // Find classes by student id
        var studentClassIdList = await _context.Classes.Select(a => a.Id).ToListAsync();
        if (model.StudentId != null)
        {
            var studentClassesObjList = await _context.StudentClasses.ToListAsync();
            var classFilterList = studentClassesObjList
                .FindAll(a => a.StudentId == model.StudentId && a.IsDelete == false)
                .Select(a => a.ClassId.Value)
                .ToList();
            if (classFilterList.Count >= 0)
            {
                studentClassIdList = classFilterList;
            }
        }

        // Find classes by admin id
        var adminClassIdList = await _context.Classes.Select(a => a.Id).ToListAsync();
        if (model.AdminId != null)
        {
            var classAccountsObjList = await _context.ClassAccounts.ToListAsync();
            var classFilterList = classAccountsObjList
                .FindAll(a => a.AdminId == model.AdminId && a.IsDelete == false)
                .Select(a => a.ClassId.Value)
                .ToList();
            if (classFilterList.Count >= 0)
            {
                adminClassIdList = classFilterList;
            }
        }

        // Find classes by trainer id
        var trainerClassIdList = await _context.Classes.Select(a => a.Id).ToListAsync();
        if (model.TrainerId != null)
        {
            var classAccountsObjList = await _context.ClassAccounts.ToListAsync();
            var classFilterList = classAccountsObjList
                .FindAll(a => a.TrainerId == model.TrainerId && a.IsDelete == false)
                .Select(a => a.ClassId.Value)
                .ToList();
            if (classFilterList.Count >= 0)
            {
                trainerClassIdList = classFilterList;
            }
        }

        var result = _context.Classes
            .Where(a => studentClassIdList.Contains(a.Id) && adminClassIdList.Contains(a.Id) &&
                        trainerClassIdList.Contains(a.Id))
            .Include(a => a.Program)
            .Include(a => a.ClassAccounts)
            .OrderBy(a => a.StartDate); // Latest class show first
        return result.ToList();
    }

    public async Task<Pagination<Class>> GetClassesByFiltersAsync(PaginationParameter paginationParameter,
        ClassesFilterModel classesFilterModel)
    {
        var filteredQuery = _context.Classes
            .Include(a => a.Program)
            .Include(a => a.ClassAccounts)
            .AsQueryable();
        filteredQuery = await ApplyFilterAndSearch(filteredQuery.Where(a => a.IsDelete == false), classesFilterModel);
        if (filteredQuery != null)
        {
            var sortedQuery = ApplySorting(filteredQuery, classesFilterModel);
            var totalCount = await sortedQuery.CountAsync();

            var classesPagination = await sortedQuery
                .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize)
                .ToListAsync();
            return new Pagination<Class>(classesPagination, totalCount, paginationParameter.PageIndex,
                paginationParameter.PageSize);
        }

        return null;
    }

    private async Task<IQueryable<Class>> ApplyFilterAndSearch(IQueryable<Class> query,
        ClassesFilterModel classesFilterModel)
    {
        if (classesFilterModel.Status != null)
        {
            // Check if classesFilterModel.Status exist in ClassStatus Enum
            if (Enum.IsDefined(typeof(ClassStatus), classesFilterModel.Status))
            {
                query = query.Where(a => a.Status == classesFilterModel.Status);
            }
        }

        if (classesFilterModel.TrainingProgramId != null)
        {
            query = query.Where(a => a.ProgramId == classesFilterModel.TrainingProgramId);
        }

        switch (classesFilterModel)
        {
            case { StartDate: not null, EndDate: not null }:
                query = query.Where(a => a.StartDate >= classesFilterModel.StartDate &&
                                         a.EndDate <= classesFilterModel.EndDate);
                break;
            case { StartDate: not null, EndDate: null }:
                query = query.Where(a => a.StartDate >= classesFilterModel.StartDate);
                break;
            case { StartDate: null, EndDate: not null }:
                query = query.Where(a => a.EndDate <= classesFilterModel.EndDate);
                break;
        }

        if (!string.IsNullOrEmpty(classesFilterModel.Search)) // Check if search is null or empty
        {
            query = query.Where(a => a.ClassName.ToLower().Contains(classesFilterModel.Search.ToLower()));
        }

        return query;
    }

    private IQueryable<Class> ApplySorting(IQueryable<Class> query, ClassesFilterModel classesFilterModel)
    {
        switch (classesFilterModel.Sort.ToLower())
        {
            case "classname":
                query = (classesFilterModel.SortDirection.ToLower() == "desc")
                    ? query.OrderByDescending(a => a.ClassName)
                    : query.OrderBy(a => a.ClassName);
                break;
            case "startdate":
                query = (classesFilterModel.SortDirection.ToLower() == "desc")
                    ? query.OrderByDescending(a => a.StartDate)
                    : query.OrderBy(a => a.StartDate);
                break;
            case "enddate":
                query = (classesFilterModel.SortDirection.ToLower() == "desc")
                    ? query.OrderByDescending(a => a.EndDate)
                    : query.OrderBy(a => a.EndDate);
                break;
            default:
                query = (classesFilterModel.SortDirection.ToLower() == "desc")
                    ? query.OrderByDescending(a => a.Id)
                    : query.OrderBy(a => a.Id);
                break;
        }

        return query;
    }
}