using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IModuleService
    {
        public Task<ResponseModel> CreateModuleAsync(CreateModuleViewModel module);
        public Task<List<ModuleDetailsModel>> GetAllModuleAsync();
        public Task<ModuleDetailsModel> GetModuleByIDAsync(int moduleId);
        public Task<ResponseModel> UpdateModuleAsync(int id, UpdateModuleViewModel module);
        public Task<ResponseModel> DeleteModuleAsync(int id);
        public Task<ResponseModel> PauseModuleAsync(int id);
        public Task<Pagination<ModuleDetailsModel>> GetPaginationAsync(PaginationParameter paginationParameter, ModuleFilterModule moduleFilterModule);
    }
}
