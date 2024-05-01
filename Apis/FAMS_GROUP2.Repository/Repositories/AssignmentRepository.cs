using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace FAMS_GROUP2.Repositories.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly FamsDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public AssignmentRepository(FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService) :
            base(
                context, timeService, claimsService)
        {
            _timeService = timeService;
            _claimsService = claimsService;
            _dbContext = context;
        }

        public async Task<Pagination<Assignment>> GetAsmsByFiltersAsync(PaginationParameter paginationParameter,
            AssignmentFilterModel asmFilterModel)
        {
            try
            {
                var asmsQuery = _dbContext.Assignments.AsQueryable();

                asmsQuery = ApplyFilterSortAndSearch(asmsQuery, asmFilterModel);

                asmsQuery = ApplySorting(asmsQuery, asmFilterModel);

                var totalCount = await asmsQuery.CountAsync();

                var asmsPagination = await asmsQuery
                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                    .Take(paginationParameter.PageSize)
                    .ToListAsync();
                return new Pagination<Assignment>(asmsPagination, totalCount, paginationParameter.PageIndex,
                    paginationParameter.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IQueryable<Assignment> ApplyFilterSortAndSearch(IQueryable<Assignment> query,
            AssignmentFilterModel asmFilterModel)
        {
            query = query.Where(asm => asm.IsDelete == false);

            if (asmFilterModel.ModuleId != null)
            {
                query = query.Where(asm => asm.ModuleId == asmFilterModel.ModuleId);
            }

            if (asmFilterModel.Status != null)
            {
                query = query.Where(asm => asm.Status.ToUpper() == asmFilterModel.Status.ToUpper());
            }

            if (asmFilterModel.StartDate != null && asmFilterModel.EndDate != null)
            {
                query = query.Where(asm =>
                    asm.StartDate >= asmFilterModel.StartDate && asm.EndDate <= asmFilterModel.EndDate);
            }
            else if (asmFilterModel.StartDate != null && asmFilterModel.EndDate == null)
            {
                query = query.Where(a => a.StartDate >= asmFilterModel.StartDate);
            }
            else if (asmFilterModel.StartDate == null && asmFilterModel.EndDate != null)
            {
                query = query.Where(a => a.EndDate <= asmFilterModel.EndDate);
            }

            if (!string.IsNullOrEmpty(asmFilterModel.Type))
            {
                query = query.Where(asm => asm.AssignmentType == asmFilterModel.Type);
            }

            if (!string.IsNullOrEmpty(asmFilterModel.Search))
            {
                query = query.Where(asm => asm.AssignmentName.Contains(asmFilterModel.Search));
            }

            return query;
        }

        private IQueryable<Assignment> ApplySorting(IQueryable<Assignment> query, AssignmentFilterModel asmFilterModel)
        {
            query = asmFilterModel.Sort.ToLower() switch
            {
                "startdate" => asmFilterModel.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(asm => asm.StartDate).ThenBy(asm => asm.AssignmentName)
                    : query.OrderBy(asm => asm.StartDate).ThenBy(asm => asm.AssignmentName),
                "enddate" => asmFilterModel.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(asm => asm.EndDate).ThenBy(asm => asm.AssignmentName)
                    : query.OrderBy(asm => asm.EndDate).ThenBy(asm => asm.AssignmentName),
                _ => asmFilterModel.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(asm => asm.Id)
                    : query.OrderBy(asm => asm.Id)
            };
            return query;
        }

        public async Task AddRangeAsyncV2(List<Assignment> assignmentList)
        {
            foreach (var entity in assignmentList)
            {
                entity.Status = AssignmentStatus.Pending.ToString();
                entity.CreatedDate = _timeService.GetCurrentTime();
                entity.CreatedBy = _claimsService.GetCurrentUserId.ToString();
                entity.IsDelete = false;
            }

            await _dbContext.Assignments.AddRangeAsync(assignmentList);
        }

        public async Task<List<string?>> GetAsmsByNameAsync(int moduleId, List<string?> listName)
        {
            var result = await _dbContext.Assignments.ToListAsync();
            return result.Where(asm => listName.Any(name => name == asm.AssignmentName?.ToUpper()) && asm.ModuleId == moduleId)
                .Select(asm => asm.AssignmentName).ToList();
        }
    }
}