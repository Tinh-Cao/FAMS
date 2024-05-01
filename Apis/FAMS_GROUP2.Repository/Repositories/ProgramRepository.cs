using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class ProgramRepository : GenericRepository<TrainingProgram>, IProgramRepository
    {
        private readonly FamsDbContext _context;

        public ProgramRepository(
            FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;

        }
        public async Task<bool> IsModuleAddedToProgramAsync(int programId, int moduleId)
        {
            return await _context.ProgramModules.AnyAsync(pm => pm.ProgramId == programId && pm.ModuleId == moduleId);
        }
        public async Task<bool> AddModuleToProgramAsync(int programId, int moduleId)
        {
            try
            {
                var programExists = await GetByIdAsync(programId);
                var moduleExists = await _context.Modules.FindAsync(moduleId);

                if (programExists == null || programExists.IsDelete == true || moduleExists == null || moduleExists.IsDelete == true)
                {
                    return false;
                }
                if (await IsModuleAddedToProgramAsync(programId, moduleId))
                {
                    return false;
                }

                var programModule = new ProgramModule
                {
                    ProgramId = programId,
                    ModuleId = moduleId,
                    IsDelete = false,
                };
                _context.ProgramModules.Add(programModule);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Pagination<ProgramResponseModel>> GetTrainingProgramsByFiltersAsync(PaginationParameter paginationParameter, ProgramFilterModel programFilterModel)
        {
            try
            {
                var tpQuery = _context.TrainingPrograms.Include(tp => tp.ProgramModules).AsQueryable();
                
                tpQuery = await ApplyFilterSortAndSearch(tpQuery, programFilterModel);
                if (tpQuery != null)
                {
                    var sortedQuery = ApplySorting(tpQuery, programFilterModel);
                    var totalCount = await sortedQuery.CountAsync();

                    var tpPagination = await sortedQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToListAsync();
                   
                    var responseModels = tpPagination.Select(tp => new ProgramResponseModel
                    {
                        Id = tp.Id.ToString(),
                        ProgramCode = tp.ProgramCode,
                        ProgramName = tp.ProgramName,
                        Duration = tp.Duration,
                        Status = tp.Status,
                        isDelete = tp.IsDelete,
                        ModulesId = tp.ProgramModules.Where(pm => pm.IsDelete == false).Select(pm => (int)pm.ModuleId).ToList()
                    }).ToList();

                    return new Pagination<ProgramResponseModel>(
                        responseModels, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                
                //return new Pagination<TrainingProgram>(tpPagination, totalCount, paginationParameter.PageIndex,
                //        paginationParameter.PageSize);
                }   

                return null;
            }
            catch (Exception)
            {
                return null;
            }
           ;
        }
        private async Task<IQueryable<TrainingProgram>> ApplyFilterSortAndSearch(IQueryable<TrainingProgram> query,
        ProgramFilterModel programFilterModel)
        {
            if (programFilterModel == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(programFilterModel.Status))
            {
                query = query.Where(a => a.Status == programFilterModel.Status);
            }

            if (programFilterModel.Duration != null)
            {
                query = query.Where(a => a.Duration == programFilterModel.Duration);
            }

            if (!string.IsNullOrEmpty(programFilterModel.Search))
            {
                query = query.Where(a => a.ProgramCode.ToLower().Contains(programFilterModel.Search.ToLower()) ||
                                         a.ProgramName.ToLower().Contains(programFilterModel.Search.ToLower()));
            }
            if (programFilterModel.isDelete != null)
            {
                query = query.Where(a => a.IsDelete == programFilterModel.isDelete);
            }
            
            return query;
        }

        private IQueryable<TrainingProgram> ApplySorting(IQueryable<TrainingProgram> query, ProgramFilterModel programFilterModel)
        {
            switch (programFilterModel.Sort.ToLower())
            {
                case "id":
                    query = (programFilterModel.SortDirection.ToLower() == "asc")
                        ? query.OrderBy(a => a.Id)
                        : query.OrderByDescending(a => a.Id);
                    break;
                case "programname":
                    query = (programFilterModel.SortDirection.ToLower() == "asc")
                        ? query.OrderBy(a => a.ProgramName)
                        : query.OrderByDescending(a => a.ProgramName);
                    break;
                case "programcode":
                    query = (programFilterModel.SortDirection.ToLower() == "asc")
                        ? query.OrderBy(a => a.ProgramCode)
                        : query.OrderByDescending(a => a.ProgramCode);
                    break;

            }

            return query;
        }
        public async Task<List<ProgramResponseModel>> GetTrainingPrograms()
        {
            var programs = await _context.TrainingPrograms
              .Include(tp => tp.ProgramModules)
              .Where(tp => tp.IsDelete==false && tp.Status == "Active")
              .ToListAsync();

            return programs.Select(tp => new ProgramResponseModel
            {
                Id = tp.Id.ToString(),
                ProgramName = tp.ProgramName,
                ProgramCode = tp.ProgramCode,
                Duration = tp.Duration,
                isDelete = tp.IsDelete,
                ModulesId = tp.ProgramModules.Where(pm => pm.IsDelete == false).Select(pm => (int)pm.ModuleId).ToList(),
            }).ToList();


        }
        public async Task<ProgramResponseModel> GetTrainingProgramById(int id)
        {
            var program =  await _context.TrainingPrograms
              .Include(tp => tp.ProgramModules)
              .FirstOrDefaultAsync(tp => tp.Id == id);
            if(program != null) { 
            return new ProgramResponseModel()
            {
                Id = program.Id.ToString(),
                ProgramName = program.ProgramName,
                ProgramCode = program.ProgramCode,
                Duration = program.Duration,
                Status = program.Status,    
                isDelete = program.IsDelete,
                ModulesId = program.ProgramModules.Where(pm => pm.IsDelete == false).Select(pm => (int)pm.ModuleId).ToList(),
            };
            }
            return null;
            //return result;
            //return programs.select(tp => new programresponsemodel
            //{
            //    id = tp.id.tostring(),
            //    programname = tp.programname,
            //    programcode = tp.programcode,
            //    duration = tp.duration,
            //    isdelete = tp.isdelete,
            //    modulesid = tp.programmodules.select(pm => (int)pm.moduleid).tolist(),
            //}).tolist();


        }
    }
}
