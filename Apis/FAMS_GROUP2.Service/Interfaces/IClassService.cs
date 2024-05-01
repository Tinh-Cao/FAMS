using System.Collections;
using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;

namespace FAMS_GROUP2.Services;

public interface IClassService
{
    public Task<ResponseDataModel<ClassItemModel>> CreateClassAsync(CreateClassModel classModel);
    public Task<IEnumerable<ClassItemModel>> GetClassesByAccountIdAsync(ClassesGetByAccountIdModel classesGetByAccountIdModel);

    public Task<Pagination<ClassItemModel>> GetClassesByFiltersAsync(PaginationParameter paginationParameter,
        ClassesFilterModel classesFilterModel);
    
    public Task<ResponseDataModel<ClassItemModel>> GetClassDetailsAsync(int classId);
    public Task<ResponseModel> UpdateClass(UpdateClassModel updateClassModel, int classId);
    public Task<ResponseDataModel<AddStudentIntoClassResponseModel>> AddStudentToClass(List<int> studentIdList, int classId);
    public Task<ResponseModel> DeleteStudentFromClass(List<int> studentIdList, int classId);
    public Task<ResponseModel> DeleteClass(int classId);
}