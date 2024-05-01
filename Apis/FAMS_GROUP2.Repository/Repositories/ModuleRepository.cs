using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        private readonly FamsDbContext _dbContext;
        public ModuleRepository(FamsDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
            )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;


        }

        public async Task<List<Module>> GetAllModuleAsyncPaging(PaginationParameter paginationParameter, ModuleFilterModule moduleFilterModule)
        {
            var allModules = _dbContext.Modules.AsQueryable();
            if (!string.IsNullOrEmpty(moduleFilterModule.Search))
            {
                allModules = allModules.Where(p => p.ModuleName!.Contains(moduleFilterModule.Search) || p.ModuleCode.Contains(moduleFilterModule.Search));
            }
            if (!string.IsNullOrEmpty(moduleFilterModule.Status))
            {
                allModules = allModules.Where(p => p.Status!.Contains(moduleFilterModule.Status));
            }
            if (moduleFilterModule.isDelete)
            {
                allModules = allModules.Where(a => a.IsDelete == moduleFilterModule.isDelete);
            }
            else
            {
                allModules = allModules.Where(a => a.IsDelete == moduleFilterModule.isDelete);
            }
            if (!string.IsNullOrEmpty(moduleFilterModule.Sort))
            {
                switch (moduleFilterModule.Sort)
                {
                    case "modulename":
                        allModules = (moduleFilterModule.SortDirection.ToLower() == "desc") ? allModules.OrderByDescending(a => a.ModuleName) : allModules.OrderBy(a => a.ModuleName);
                        break;
                    default:
                        allModules = (moduleFilterModule.SortDirection.ToLower() == "desc") ? allModules.OrderByDescending(a => a.Id) : allModules.OrderBy(a => a.Id);
                        break;
                }
            }
            var modules = await allModules.ToListAsync();

            var modulesQuery = modules
                           .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                           .Take(paginationParameter.PageSize).ToList();

            return modulesQuery;
        }

        public async Task<bool> isModuleUsed(int id)
        {
            bool isModuleUsed = await _dbContext.ProgramModules.AnyAsync(pm => pm.ModuleId == id);
            return isModuleUsed;
        }
    }
}
