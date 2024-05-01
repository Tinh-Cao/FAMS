using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IScoreRepository : IGenericRepository<Score>
    {
        Task<Score> GetScoreIdAsync(int studentId, int classId);

        Task<List<Score>> GetScoreIds(List<int> StudentIds, List<int> ClassId);

        Task<List<Score>> GetScoreIdsV1(List<ScoreImportModel> scores);

        Task<string> GetName(int studentId);

        Task<Pagination<Score>> GetScoresByFiltersAsync(PaginationParameter paginationParameter, ScoreFilterModel scoreFilterModel);

        Task<List<Score>> GetScoreIdsV2(List<int> StudentIds, int ClassId);

        Task<List<Student>> GetStudents(List<ScoreImportModel> scores);
    }
}
