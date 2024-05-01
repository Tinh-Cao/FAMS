using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.Utils;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttendanceClassService _attendanceClassService;


        public StudentService(IMapper mapper, IClaimsService claimsService, ICurrentTime currentTime, IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._claimsService = claimsService;
            this._currentTime = currentTime;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ImportStudentResponseModel> AddRangeStudent(List<StudentImportModel> students)
        {
            try
            {
                var studentImportList = new List<Student>();
                var response = new ImportStudentResponseModel();
                var existingStudent = await _unitOfWork.StudentRepository.GetAllAsync();
                foreach (StudentImportModel newsStudent in students)
                {
                    //var studentChecking = await _unitOfWork.StudentRepository.FindExistingStudent(newsStudent.Email, newsStudent.StudentCode);
                    var studentChecking = existingStudent.FirstOrDefault(x => x.Email == newsStudent.Email && x.StudentCode == newsStudent.StudentCode);

                    if (studentChecking != null)
                    {
                        if (response.DuplicatedStudents == null)
                        {
                            response.DuplicatedStudents = new List<StudentImportModel>();
                        }
                        response.DuplicatedStudents.Add(newsStudent);
                    }
                    else
                    {
                        var newStudent = new Student
                        {
                            FullName = newsStudent.FullName,
                            Email = newsStudent.Email,
                            PhoneNumber = newsStudent.PhoneNumber,
                            Dob = newsStudent.Dob,
                            Gender = newsStudent.Gender.ToLower() == "male",
                            University = newsStudent.University,
                            Major = newsStudent.Major,
                            YearOfGraduation = newsStudent.YearOfGraduation,
                            Gpa = newsStudent.Gpa,
                            StudentCode = newsStudent.StudentCode,
                            EnrollmentArea = newsStudent.EnrollmentArea,
                            Status = StudentStatus.OffClass.ToString(),
                            Skill = newsStudent.Skill,
                            UnsignFullName = StringTools.ConvertToUnSign(newsStudent.FullName),
                            CreatedDate = _currentTime.GetCurrentTime(),
                            CreatedBy = _claimsService.GetCurrentUserId.ToString(),
                        };
                        studentImportList.Add(newStudent);
                    }

                }

                if (studentImportList.Count > 0)
                {
                    await _unitOfWork.StudentRepository.AddRangeAsync(studentImportList);
                    await _unitOfWork.SaveChangeAsync();
                    if (response.DuplicatedStudents != null)
                    {
                        return new ImportStudentResponseModel
                        {
                            DuplicatedStudents = response.DuplicatedStudents,
                            AddedStudents = _mapper.Map<List<StudentImportModel>>(studentImportList),
                            Message = "These student have been successfully added and some students are existed",
                            Status = true
                        };
                    }
                    else
                    {
                        return new ImportStudentResponseModel
                        {
                            AddedStudents = _mapper.Map<List<StudentImportModel>>(studentImportList),
                            Message = "These student have been successfully added",
                            Status = true
                        };
                    }
                }
                if (response.DuplicatedStudents != null)
                {
                    response.Message = "Importing process have duplicated problems";
                    response.Status = true;
                    return response;
                }
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<StudentDetailsModel> GetStudentDetailsAsync(int studentId)
        {
            try
            {
                var student = await _unitOfWork.StudentRepository.GetStudentAsync(studentId);
                if (student != null)
                {
                    string genderString = student.Gender == true ? "Male" : "Female";
                    var accountDetails = _mapper.Map<StudentDetailsModel>(student);
                    accountDetails.Gender = genderString;
                    return accountDetails;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pagination<StudentDetailsModel>> GetStudentsByFiltersAsync(PaginationParameter paginationParameter, StudentFilterModel accountFilterModel)
        {
            var students = await _unitOfWork.StudentRepository.GetStudentsByFiltersAsync(paginationParameter, accountFilterModel);
            var mappedResult = new List<StudentDetailsModel>();
            // nơi xử lý gender 

            foreach (var model in students)
            {
                var mapper = _mapper.Map<StudentDetailsModel>(model);
                mapper.Gender = model.Gender == true ? "Male" : "Female";
                mappedResult.Add(mapper);
            }

            return new Pagination<StudentDetailsModel>(mappedResult, students.TotalCount, students.CurrentPage, students.PageSize);

        }

        public async Task<ResponseLoginModel> LoginGoogleStudentAsync(string credential)
        {
            var result = await _unitOfWork.StudentRepository.LoginGoogleAsync(credential);
            if (await _unitOfWork.SaveChangeAsync() > 0 && result != null)
            {
                return result;
            }
            else
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Your account is not allowed login to the system, please contact to your Admin",
                }; ;
            }

        }

        public async Task<StudentDetailsModel> UpdateStudentAsync(int studentId, StudentUpdateModel updateStudent)
        {
            var existingStudent = await _unitOfWork.StudentRepository.GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent = _mapper.Map(updateStudent, existingStudent);
                var updatedAccount = await _unitOfWork.StudentRepository.Update(existingStudent);
                await _unitOfWork.SaveChangeAsync();
                var result = _mapper.Map<StudentDetailsModel>(existingStudent);
                result.Gender = updateStudent.Gender.ToLower() == "true" ? "Male" : "Female";
                return result;
            }
            //return new ResponseModel
            //{
            //    Status = false,
            //    Message = "Account updated unsuccessfully!"
            //};
            return null;
        }

        public async Task<List<StudentDetailsModel>> DeleteStudentAsync(List<int> studentIds)
        {
            var list = new List<Student>();

            foreach (int studentId in studentIds)
            {
                var result = await _unitOfWork.StudentRepository.GetStudentAsync(studentId);
                if (result != null)
                {
                    list.Add(result);
                }
                else
                {
                    return null;
                }
            }

            if (list.Count > 0)
            {
                list = _unitOfWork.StudentRepository.DeleteRangeStudentAsync(list);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<List<StudentDetailsModel>>(list);
            }
            return null;
        }

        public async Task<List<StudentDetailsModel>> UnBanStudentAsync(List<int> studentIds)
        {
            var list = new List<Student>();

            foreach (int studentId in studentIds)
            {
                var result = await _unitOfWork.StudentRepository.GetStudentAsync(studentId);
                if (result != null)
                {
                    list.Add(result);
                }
                else
                {
                    return null;
                }
            }

            if (list.Count > 0)
            {
                list = _unitOfWork.StudentRepository.UnbanRangeStudentAsync(list);
                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<List<StudentDetailsModel>>(list);
            }
            return null;
        }

        public async Task<List<StudentDetailsModel>> GetStudentBySpecificClass(int classId)
        {
            try
            {
                var students = await _unitOfWork.StudentClassRepository.getStudentFromAClass(classId);
                var mappedResult = new List<StudentDetailsModel>();
                // nơi xử lý gender 
                foreach (var model in students)
                {
                    var mapper = _mapper.Map<StudentDetailsModel>(model);
                    mapper.Gender = model.Gender == true ? "Male" : "Female";
                    mappedResult.Add(mapper);
                }
                return mappedResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseDataModel<AddStudentIntoClassResponseModel>> AddStudentIntoClass(List<int> studentIdList, int classId)
        {
            using (var transaction = _unitOfWork.DbContext.Database.BeginTransaction())
            {
                var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
                if (classObj == null)
                {
                    classObj = new Class
                    {
                        ProgramId = 1,
                        ClassName = "New Class", // Đặt tên lớp học tạm thời
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(3), // Đặt thời gian kết thúc tạm thời
                        Location = "Online", // Địa điểm tạm thời
                        Status = "Active"
                    };
                    await _unitOfWork.ClassRepository.AddAsync(classObj);
                }

                var invalidStudentId = new List<int>();
                foreach (var item in studentIdList)
                {
                    var studentObj = await _unitOfWork.StudentRepository.GetByIdAsync(item);
                    if (studentObj == null) invalidStudentId.Add(item);
                }

                if (invalidStudentId.Count > 0)
                {
                    return new ResponseDataModel<AddStudentIntoClassResponseModel>()
                    {
                        Status = false,
                        Message = "Student(s) does not exist",
                        Data = new AddStudentIntoClassResponseModel()
                        {
                            InvalidStudentId = invalidStudentId
                        }
                    };
                }

                var existedStudent = new List<StudentDetailsModel>();
                foreach (var studentId in studentIdList)
                {
                    bool valid = await _attendanceClassService.InitialStudentClassOfStudent(classObj.Id, studentId);
                    if (!valid) existedStudent.Add(_mapper.Map<StudentDetailsModel>(await _unitOfWork.StudentRepository.GetStudentAsync(studentId)));
                }

                await _unitOfWork.SaveChangeAsync();
                transaction.Commit();

                var response = new ResponseGenericModel<List<StudentDetailsModel>>(existedStudent, true, "lmao");

                return new ResponseDataModel<AddStudentIntoClassResponseModel>()
                {
                    Status = true,
                    Message = "Add students into class successfully",
                };
            }
        }

    }
}
