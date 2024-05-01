using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FAMS_GROUP2.Services.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;

namespace FAMS_GROUP2.Services;

public class ClassService : IClassService
{
    private readonly IMapper _mapper;
    private readonly IAttendanceClassService _attendanceClassService;
    private readonly IStudentService _studentService;
    private readonly IUnitOfWork _unitOfWork;

    public ClassService(IMapper mapper, IAttendanceClassService attendanceClassService, IStudentService studentService,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _attendanceClassService = attendanceClassService;
        _studentService = studentService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDataModel<ClassItemModel>> CreateClassAsync(CreateClassModel createClassModel)
    {
        // Create new class (add programId if needed)
        var classObj = _mapper.Map<Class>(createClassModel);
        if (createClassModel.ProgramId != null)
        {
            var trainingProgramObj = await _unitOfWork.ProgramRepository.GetByIdAsync((int)createClassModel.ProgramId);
            if (trainingProgramObj == null)
            {
                return new ResponseDataModel<ClassItemModel>()
                {
                    Status = false,
                    Message = "Training program id does not exist"
                };
            }
        }

        classObj.IsDelete = false;
        classObj.Status = ClassStatus.Ongoing.ToString();
        await _unitOfWork.ClassRepository.AddAsync(classObj);
        if (await _unitOfWork.SaveChangeAsync() == 0)
        {
            return new ResponseDataModel<ClassItemModel>()
            {
                Status = false,
                Message = "Error creating new class"
            };
        }

        //Create new ClassAccount (add admin if needed)
        var classAccountObj = new ClassAccount()
        {
            Class = classObj
        };
        if (createClassModel.AdminId != null)
        {
            var adminAccount =
                await _unitOfWork.AccountRepository.GetAccountDetailsAsync((int)createClassModel.AdminId);
            if (adminAccount == null)
            {
                return new ResponseDataModel<ClassItemModel>()
                {
                    Status = false,
                    Message = "Admin id does not exist"
                };
            }

            classAccountObj.Admin = adminAccount;
        }

        await _unitOfWork.ClassAccountRepository.AddAsync(classAccountObj);
        if (await _unitOfWork.SaveChangeAsync() == 0)
        {
            return new ResponseDataModel<ClassItemModel>()
            {
                Status = false,
                Message = "Error adding admin into class"
            };
        }
        
        await _attendanceClassService.InitialStudentClassOfClass(classObj.Id); // Attendance

        return new ResponseDataModel<ClassItemModel>()
        {
            Status = true,
            Message = "Create new class successfully",
            Data = _mapper.Map<ClassItemModel>(classObj)
        };
    }

    public async Task<IEnumerable<ClassItemModel>> GetClassesByAccountIdAsync(
        ClassesGetByAccountIdModel classesGetByAccountIdModel)
    {
        if (classesGetByAccountIdModel.StudentId == null && classesGetByAccountIdModel.TrainerId == null &&
            classesGetByAccountIdModel.AdminId == null)
        {
            return null;
        }

        var classList = await _unitOfWork.ClassRepository.GetAllByAccountId(classesGetByAccountIdModel);
        var mappedResult = _mapper.Map<List<ClassItemModel>>(classList);

        return mappedResult;
    }

    public async Task<Pagination<ClassItemModel>> GetClassesByFiltersAsync(PaginationParameter paginationParameter,
        ClassesFilterModel classesFilterModel)
    {
        var classList =
            await _unitOfWork.ClassRepository.GetClassesByFiltersAsync(paginationParameter, classesFilterModel);
        var mappedResult = _mapper.Map<List<ClassItemModel>>(classList);

        return new Pagination<ClassItemModel>(mappedResult, classList.TotalCount, classList.CurrentPage,
            classList.PageSize);
    }

    public async Task<ResponseDataModel<ClassItemModel>> GetClassDetailsAsync(int classId)
    {
        var classObj = await _unitOfWork.ClassRepository.GetClassDetail(classId);
        var result = _mapper.Map<ClassItemModel>(classObj);
        if (classObj.ProgramId != null)
        {
            var programObj = await _unitOfWork.ProgramRepository.GetByIdAsync((int)classObj.ProgramId);
            result.ProgramName = programObj?.ProgramName;
            result.ProgramCode = programObj?.ProgramCode;
        }

        var classAccountObject = await _unitOfWork.ClassAccountRepository.GetClassAccountByClassId(classId);
        result.AdminId = classAccountObject.AdminId;
        result.TrainerId = classAccountObject.TrainerId;

        return new ResponseDataModel<ClassItemModel>()
        {
            Status = true,
            Data = result
        };
    }

    public async Task<ResponseModel> UpdateClass(UpdateClassModel updateClassModel, int classId)
    {
        // Update class
        var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
        if (classObj == null)
        {
            return new ResponseModel()
            {
                Status = false,
                Message = "Class does not exist"
            };
        }

        if (updateClassModel.ProgramId == null)
        {
            classObj.ProgramId = null;
        }
        else if (updateClassModel.ProgramId != classObj.ProgramId)
        {
            var trainingProgramObj = await _unitOfWork.ProgramRepository.GetByIdAsync((int)updateClassModel.ProgramId);
            if (trainingProgramObj == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Training program does not exist"
                };
            }

            classObj.ProgramId = updateClassModel.ProgramId;
        }

        // Update class account
        var classAccountObj = await _unitOfWork.ClassAccountRepository.GetClassAccountByClassId(classId);
        if (updateClassModel.AdminId != null && updateClassModel.AdminId != classAccountObj.AdminId)
        {
            var adminObj =
                await _unitOfWork.AccountRepository.GetAccountDetailsAsync((int)updateClassModel.AdminId);
            if (adminObj == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Admin account does not exist"
                };
            }

            classAccountObj.AdminId = adminObj.Id;
        }
        else if (updateClassModel.AdminId == null)
        {
            classAccountObj.AdminId = null;
        }

        if (updateClassModel.TrainerId != null && updateClassModel.TrainerId != classAccountObj.TrainerId)
        {
            var trainerObj =
                await _unitOfWork.AccountRepository.GetAccountDetailsAsync((int)updateClassModel.TrainerId);
            if (trainerObj == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Trainer account does not exist"
                };
            }

            classAccountObj.TrainerId = trainerObj.Id;
        }
        else if (updateClassModel.TrainerId == null)
        {
            classAccountObj.TrainerId = null;
        }

        await _unitOfWork.ClassAccountRepository.Update(classAccountObj);
        classObj.ClassName = updateClassModel.ClassName;
        classObj.StartDate = updateClassModel.StartDate;
        classObj.EndDate = updateClassModel.EndDate;
        classObj.Location = updateClassModel.Location;
        classObj.Status = updateClassModel.Status;
        await _unitOfWork.ClassRepository.Update(classObj);
        if (await _unitOfWork.SaveChangeAsync() == 0)
        {
            return new ResponseModel()
            {
                Status = false,
                Message = "Error updating class"
            };
        }

        return new ResponseModel()
        {
            Status = true,
            Message = "Update class successfully",
        };
    }

    public async Task<ResponseModel> DeleteClass(int classId)
    {
        var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
        if (classObj == null)
        {
            return new ResponseModel()
            {
                Status = false,
                Message = "Class does not exist"
            };
        }

        var numberOfStudent = await _unitOfWork.StudentClassRepository.GetNumberOfStudent(classId);
        if (numberOfStudent > 0)
        {
            return new ResponseModel()
            {
                Status = false,
                Message = "The class cannot be deleted because there are students in this class"
            };
        }

        await _attendanceClassService.DeleteAllStudentOfClass(classId); // Delete all attendance records
        await _unitOfWork.ClassRepository.SoftRemove(classObj);
        if (await _unitOfWork.SaveChangeAsync() == 0)
        {
            return new ResponseModel()
            {
                Status = false,
                Message = "Error deleting class"
            };
        }

        return new ResponseModel()
        {
            Status = true,
            Message = "Delete class successfully"
        };
    }
    public async Task<ResponseDataModel<AddStudentIntoClassResponseModel>> AddStudentToClass(List<int> studentIdList, int classId)
    {
        var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
        if (classObj == null)
        {
            return new ResponseDataModel<AddStudentIntoClassResponseModel>()
            {
                Status = false,
                Message = "Class does not exist"
            };
        }

        var invalidStudentId = new List<int>();
        var existedStudent = new List<StudentDetailsModel>();
        bool flagCheck = false;
        var students = await _unitOfWork.StudentRepository.GetAllAsync();

        using (var transaction = _unitOfWork.DbContext.Database.BeginTransaction())
        {
            try
            {
                foreach (var item in studentIdList)
                {
                    var studentObj = students.FirstOrDefault(x => x.Id == item);
                    if (studentObj == null)
                    {
                        invalidStudentId.Add(item);
                        flagCheck = true;
                    }
                    else
                    {
                        if (!flagCheck)
                        {
                            bool valid = await _attendanceClassService.InitialStudentClassOfStudent(classObj.Id, item);
                            if (!valid) existedStudent.Add(_mapper.Map<StudentDetailsModel>(studentObj));
                        }

                    }
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Commit();
            }

        }

        if (invalidStudentId.Count > 0)
        {
            return new ResponseDataModel<AddStudentIntoClassResponseModel>()
            {
                Status = false,
                Message = "Student(s) does not exist, can not accomplish function",
                Data = new AddStudentIntoClassResponseModel()
                {
                    InvalidStudentId = invalidStudentId
                }
            };
        }

        using (var transaction = _unitOfWork.DbContext.Database.BeginTransaction())
        {
            try
            {
                foreach (var studentId in studentIdList)
                {
                    bool valid = await _attendanceClassService.InitialStudentClassOfStudent(classObj.Id, studentId);
                    if (!valid) existedStudent.Add(await _studentService.GetStudentDetailsAsync(studentId));
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Commit();
            }
            
        }
            

        return new ResponseDataModel<AddStudentIntoClassResponseModel>()
        {
            Status = true,
            Message = existedStudent.Count == studentIdList.Count ? "These students is already in class" : "Add student to class successfully",
            Data = existedStudent.Count > 0 ? new AddStudentIntoClassResponseModel()
            {
                ExistedStudent = existedStudent
            } : null  
        };
    }

    public async Task<ResponseModel> DeleteStudentFromClass(List<int> studentIdList, int classId)
    {
        var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
        if (classObj == null)
        {
            return new ResponseModel()
            {
                Status = false,
                Message = "Class does not exist"
            };
        }

        using (var transaction = _unitOfWork.DbContext.Database.BeginTransaction())
        {
            try
            {
                foreach (var studentId in studentIdList)
                {
                    await _attendanceClassService.DeleteStudentClassOfStudent(classObj.Id, studentId);
                }
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Commit();
            }
        }


        return new ResponseModel()
        {
            Status = true,
            Message = "Delete student from class successfully"
        };
    }
}