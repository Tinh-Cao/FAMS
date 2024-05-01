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

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IModuleRepository: IGenericRepository<Module>
    {
        
        public Task<List<Module>> GetAllModuleAsyncPaging(PaginationParameter paginationParameter, ModuleFilterModule moduleFilterModule);
        public Task<bool> isModuleUsed(int id);
    }
}
