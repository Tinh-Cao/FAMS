using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        public Task<Pagination<Lesson>> GetLessonByFilterAsync(PaginationParameter paginationParameter, LessonFilterModel lessonFilterModel);
    }
}
