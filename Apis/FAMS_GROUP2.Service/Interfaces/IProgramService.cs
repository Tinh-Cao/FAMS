using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;

namespace FAMS_GROUP2.Services.Interfaces
{
     public interface IProgramService
    {
        public Task<TrainingProgram> CreateProgramAsync(ProgramModel program);
        public Task<List<ProgramResponseModel>> GetAllProgramAsync();
        public Task<ProgramResponseModel> GetProgramByIdAsync(int id);

        public Task<int> UpdateProgramAsync(int id, UpdateProgramModel upprogram);

        public Task<int> UpdateProgramStatusAsync(int id);

        public Task<int> DeleteProgramAsync(int id);
        public Task<Pagination<ProgramResponseModel>> GetProgramsByFiltersAsync(PaginationParameter paginationParameter,
        ProgramFilterModel programFilterModel);
        public Task<bool> AddModuleToProgramAsync(int programId, int moduleId);
    }
}
