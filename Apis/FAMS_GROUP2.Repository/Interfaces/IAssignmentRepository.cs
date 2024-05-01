using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IAssignmentRepository : IGenericRepository<Assignment>
    {
        Task<Pagination<Assignment>> GetAsmsByFiltersAsync(PaginationParameter paginationParameter, AssignmentFilterModel asmFilterModel);
        Task AddRangeAsyncV2(List<Assignment> assignmentList);
        Task<List<string?>> GetAsmsByNameAsync(int moduleId, List<string?> asmList);
    }
}
