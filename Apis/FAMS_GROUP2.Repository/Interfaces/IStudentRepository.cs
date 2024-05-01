using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IStudentRepository: IGenericRepository<Student>
    {
        Task<ResponseModel> AddStudentManually(StudentImportModel student);
        Task<ResponseLoginModel> LoginGoogleAsync(string credential);

        Task<Pagination<Student>> GetStudentsByFiltersAsync(PaginationParameter paginationParameter, StudentFilterModel accountFilterModel);

        Task<Student> FindByEmailAsync(string email);
        Task<int> UpdateTokenAsync(Student student);
        Task<Student> GetStudentAsync(int accountId);

        Task<Boolean> CheckByEmailAndIdAsync(string email, int id);
        Task<Student> DeleteStudentAsync(Student student);
        List<Student> DeleteRangeStudentAsync(List<Student> students);
        Task<Student> FindExistingStudent(string email, string studentCode);
        List<Student> UnbanRangeStudentAsync(List<Student> students);
        Task<Student> UpdateStudentOnBoardStatus(int studentId);
        Task<Student> UpdateStudentOffClass(int studentId);
        Task<Student> UpdateStudentInClass(int studentId);
        Task<Student> GetStudentWithScoreAndClassWithTrainingProgramAsync(int studentId, int classId);
    }
}
