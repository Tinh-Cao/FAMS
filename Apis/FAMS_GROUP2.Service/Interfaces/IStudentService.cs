using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Pagination<StudentDetailsModel>> GetStudentsByFiltersAsync(PaginationParameter paginationParameter, StudentFilterModel accountFilterModel);

        Task<ResponseLoginModel> LoginGoogleStudentAsync(string credential);

        Task<StudentDetailsModel> GetStudentDetailsAsync(int studentId);


        Task<ImportStudentResponseModel> AddRangeStudent(List<StudentImportModel> students);
        Task<StudentDetailsModel> UpdateStudentAsync(int studentId, StudentUpdateModel student);
        Task<List<StudentDetailsModel>> DeleteStudentAsync(List<int> studentIds);
        Task<List<StudentDetailsModel>> GetStudentBySpecificClass(int classId);
        Task<List<StudentDetailsModel>> UnBanStudentAsync(List<int> studentIds);
    }
}
