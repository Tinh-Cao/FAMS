using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;

namespace FAMS_GROUP2.Repositories;

public interface IClassRepository : IGenericRepository<Class>
{
    public Task<Class> GetClassDetail(int classId);
    public Task<List<Class>> GetAllByAccountId(ClassesGetByAccountIdModel model);

    public Task<Pagination<Class>> GetClassesByFiltersAsync(PaginationParameter paginationParameter,
        ClassesFilterModel classesFilterModel);
}