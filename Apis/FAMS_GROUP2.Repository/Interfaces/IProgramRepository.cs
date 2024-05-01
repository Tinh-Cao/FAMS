using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IProgramRepository : IGenericRepository<TrainingProgram>
    {
        public Task<Pagination<ProgramResponseModel>> GetTrainingProgramsByFiltersAsync(PaginationParameter paginationParameter,
          ProgramFilterModel programFilterModel);
        public Task<bool> AddModuleToProgramAsync(int programId, int moduleId);

        public Task<List<ProgramResponseModel>> GetTrainingPrograms() ;
        public Task<ProgramResponseModel> GetTrainingProgramById(int id);    }
}
