using FAMS_GROUP2.Repositories.ViewModels.StudentModels;

namespace FAMS_GROUP2.Repositories.ViewModels.ResponseModels;

public class AddStudentIntoClassResponseModel
{
    public List<int>? InvalidStudentId { get; set; }
    public List<StudentDetailsModel>? ExistedStudent { get; set; }
}