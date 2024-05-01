using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<AssignmentResponseModel> CreateAsmByExcelAsync(int moduleId, List<AssignmentImportModel> listModel);
        Task<Pagination<AssignmentViewModel>> GetAsmsByFiltersAsync(PaginationParameter paginationParameter, AssignmentFilterModel asmFilterModel);
        Task<ResponseModel> UpdateAsmAsync(int id, AssignmentImportModel model);
        Task<AssignmentViewModel?> GetAsmById(int id);
        Task<ResponseModel> SoftDeleteAsmById(int id);
    }
}
