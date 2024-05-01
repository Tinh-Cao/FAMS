using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IScoreService
    {
        Task<ScoreViewModel> GetScoreByIdAsync(int StudentId, int ClassId);

        Task<ResponseModel> AddScoreByFormAsync(ScoreCreateModel scoreModel);

        Task<ResponseModel> UpdateScoreByStudentAsync(int StudentId, int ClassId, ScoreUpdateModel scoreModel);

        Task<ResponseModel> UpdateScoreByClassAsync(List<ScoreCreateModel> scoreModel);

        Task<ResponseModel> DeleteScoreAsync(int StudentId, int ClassId);

        Task<ResponseModel> DeleteScoresByClassAsync(List<int> StudentIds, int ClassIds);

        Task<ScoreImportResponseModel> ScoreImportExcel(List<ScoreImportModel> scoreModel);

        Task<Pagination<ScoreViewModel>> GetScoresByFiltersAsync(PaginationParameter paginationParameter, ScoreFilterModel scoreFilterModel);
    }
}
