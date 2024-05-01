using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface ILessonService
    {
        public Task<ResponseModel> CreateLessonAsync(CreateLessonModel lessonModel);
        public Task<List<Lesson>> GetAllLessonAsync();
        public Task<Lesson> GetLessonByIdAsync(int id);
        public Task<ResponseModel> UpdateLessonAsync(int id, UpdateLessonModel lesson);
        public Task<ResponseModel> DeleteLessonAsync(int id);
        public Task<Pagination<LessonDetailsModel>> GetLessonByFilterAsync (PaginationParameter paginationParameter, LessonFilterModel lessonFilterModel);
    }
}
