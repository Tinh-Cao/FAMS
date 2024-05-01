using Application.Utils;
using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Utils;
using FAMS_GROUP2.Repositories.ViewModels;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly FamsDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        public StudentRepository(FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService, IConfiguration configuration) : base(
      context, timeService, claimsService)
        {
            _dbContext = context;
            _configuration = configuration;
            _claimsService = claimsService;
            _currentTime = timeService;

        }

        public async Task<ResponseModel> AddStudentManually(StudentImportModel newStudent)
        {


            try
            {
                var studentChecking = await _dbContext.Students.AnyAsync(x => x.Email == newStudent.Email && x.StudentCode == newStudent.StudentCode);
                if (studentChecking)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "Email or student code are already existed!"
                    };
                }

                var student = new Student
                {
                    FullName = newStudent.FullName,
                    Email = newStudent.Email,
                    PhoneNumber = newStudent.PhoneNumber,
                    Dob = newStudent.Dob,
                    Gender = newStudent.Gender.ToLower() == "male",
                    University = newStudent.University,
                    Major = newStudent.Major,
                    YearOfGraduation = newStudent.YearOfGraduation,
                    Gpa = newStudent.Gpa,
                    StudentCode = newStudent.StudentCode,
                    EnrollmentArea = newStudent.EnrollmentArea,
                    Status = StudentStatus.OffClass.ToString(),
                    Skill = newStudent.Skill,
                    CreatedDate = _currentTime.GetCurrentTime(),
                    CreatedBy = _claimsService.GetCurrentUserId.ToString(),
                    Image = newStudent.Image,
                    UnsignFullName = StringTools.ConvertToUnSign(newStudent.FullName)
                };

                _dbContext.Students.Add(student);

                return new ResponseModel
                {
                    Status = true,
                    Message = "successful"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = ex.Message
                };
            }


        }

        public async Task<Pagination<Student>> GetStudentsByFiltersAsync(PaginationParameter paginationParameter, StudentFilterModel accountFilterModel)
        {
            try
            {
                var queryFilter = _dbContext.Students.AsNoTracking().AsQueryable(); //
                queryFilter = ApplyFilter(queryFilter, accountFilterModel);
                var totalCount = await queryFilter.CountAsync();

                if ( totalCount > 0)
                {
                    var studentsSort = ApplySorting(queryFilter, accountFilterModel);
                    var StudentsPagination = studentsSort
                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                    .Take(paginationParameter.PageSize);

                    return new Pagination<Student>(StudentsPagination.ToList(), totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                    
                }
                return new Pagination<Student>(Enumerable.Empty<Student>().ToList(), totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Student> FindByEmailAsync(string email)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Student> FindExistingStudent(string email, string studentCode)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(a => a.Email == email && a.StudentCode == studentCode);

        }

        public async Task<Boolean> CheckByEmailAndIdAsync(string email, int id)
        {
            return await _dbContext.Students.AnyAsync(a => a.Email == email || a.Id == id);
        }
        public async Task<int> UpdateTokenAsync(Student student)
        {
            _dbContext.Update(student);
            // await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<ResponseLoginModel> LoginGoogleAsync(string credential)
        {
            string clientId1 = _configuration["GoogleCredential1:ClientId"];
            string clientId2 = _configuration["GoogleCredential2:ClientId"];

            if (string.IsNullOrEmpty(clientId1) && string.IsNullOrEmpty(clientId2))
            {
                throw new Exception("ClientId is null!");
            }

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { clientId1, clientId2 }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
            if (payload == null)
            {
                throw new Exception("Credential incorrect!");
            }

            var accountExist = await FindByEmailAsync(payload.Email);

            if (accountExist != null && accountExist.IsDelete == false)
            {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, accountExist.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                authClaims.Add(new Claim(ClaimTypes.Role, "Student"));

                var refreshToken = TokenTools.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                accountExist.RefreshToken = refreshToken;
                accountExist.RefreshTokenExpiryTime = _currentTime.GetCurrentTime().AddDays(refreshTokenValidityInDays);

                await UpdateTokenAsync(accountExist);

                var token = GenerateJWTToken.CreateToken(authClaims, _configuration, DateTime.UtcNow);
                return new ResponseLoginModel
                {
                    Status = true,
                    Message = "Login successfully.",
                    JWT = new JwtSecurityTokenHandler().WriteToken(token),
                    Expired = token.ValidTo.ToLocalTime(),
                    JWTRefreshToken = refreshToken,
                };
            }
            else
            {
                return null;
            }

        }

        private IQueryable<Student> ApplyFilter(IQueryable<Student> query, StudentFilterModel accountFilterModel)
        {
            if (accountFilterModel == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(accountFilterModel.Search))
            {
                query = query.Where(a =>
                  a.FullName.Contains(accountFilterModel.Search) ||
                  a.UnsignFullName.Contains(accountFilterModel.Search)
              );
            }

            if (!string.IsNullOrEmpty(accountFilterModel.Gender))
            {
                bool isMale = accountFilterModel.Gender.ToLower() == "male";
                query = query.Where(a => a.Gender == isMale);
            }
            if (!string.IsNullOrEmpty(accountFilterModel.Skill))
            {
                query = query.Where(a =>
                    a.Skill.Contains(accountFilterModel.Skill)
                );
            }


            if (!string.IsNullOrEmpty(accountFilterModel.Status))
            {
                query = query.Where(a =>
                  a.Status.Contains(accountFilterModel.Status)
              );
            }


            if (accountFilterModel.isDelete == true)
            {
                query = query.Where(a => a.IsDelete == accountFilterModel.isDelete);
            }
            else if (accountFilterModel.isDelete == false)
            {
                query = query.Where(a => a.IsDelete == accountFilterModel.isDelete);

            }

            if (accountFilterModel.ClassId != null)
            {
                var studentInclass = _dbContext.StudentClasses
                    .Where(sc => sc.ClassId == accountFilterModel.ClassId && sc.IsDelete == false && sc.Student != null)
                    .Join(query, sc => sc.StudentId, s => s.Id, (sc, s) => s); // prepare statement sql
                query = query.Except(studentInclass);
            }

            return query;
        }

        private IQueryable<Student> ApplySorting(IQueryable<Student> query, StudentFilterModel accountFilterModel)
        {
            switch (accountFilterModel.Sort.ToLower())
            {
                case "fullname":
                    query = (accountFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.FullName) : query.OrderByDescending(a => a.FullName);
                    break;
                case "dob":
                    query = (accountFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Dob) : query.OrderByDescending(a => a.Dob);
                    break;
                default:
                    query = (accountFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                    break;
            }

            return query;
        }

        public async Task<Student> GetStudentAsync(int accountId)
        {
            var account = await _dbContext.Students.SingleOrDefaultAsync(x => x.Id == accountId);
            return account;
        }

        public async Task<Student> DeleteStudentAsync(Student student)
        {
            student.IsDelete = true;
            student.DeletedDate = _currentTime.GetCurrentTime();
            student.DeletedBy = _claimsService.GetCurrentUserId.ToString();
            _dbContext.Entry(student).State = EntityState.Modified;
            // await _dbContext.SaveChangesAsync();
            return student;
        }

        public List<Student> DeleteRangeStudentAsync(List<Student> students)
        {
            try
            {
                foreach (var student in students)
                {
                    student.IsDelete = true;
                    student.DeletedDate = _currentTime.GetCurrentTime();
                    student.DeletedBy = _claimsService.GetCurrentUserId.ToString();
                    _dbContext.Entry(student).State = EntityState.Modified;
                    // await _dbContext.SaveChangesAsync();
                }

                return students;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Student> UnbanRangeStudentAsync(List<Student> students)
        {
            try
            {
                foreach (var student in students)
                {
                    if (student.IsDelete == true)
                    {
                        student.IsDelete = false;
                    }
                    student.DeletedDate = _currentTime.GetCurrentTime();
                    student.DeletedBy = _claimsService.GetCurrentUserId.ToString();
                    _dbContext.Entry(student).State = EntityState.Modified;
                    // await _dbContext.SaveChangesAsync();
                }

                return students;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Student> UpdateStudentInClass(int studentId)
        {
            var student = await GetStudentAsync(studentId);
            //update 
            student.Status = StudentStatus.InClass.ToString();
            student.ModifiedDate = _currentTime.GetCurrentTime();
            student.ModifiedBy = _claimsService.GetCurrentUserId.ToString();
            _dbContext.Entry(student).State = EntityState.Modified;
            // await _dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentOffClass(int studentId)
        {
            var student = await GetStudentAsync(studentId);
            //update 
            student.Status = StudentStatus.OffClass.ToString();
            student.ModifiedDate = _currentTime.GetCurrentTime();
            student.ModifiedBy = _claimsService.GetCurrentUserId.ToString();
            _dbContext.Entry(student).State = EntityState.Modified;
            // await _dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentOnBoardStatus(int studentId)
        {
            var student = await GetStudentAsync(studentId);
            //update 
            student.Status = StudentStatus.OffClass.ToString();
            student.ModifiedDate = _currentTime.GetCurrentTime();
            student.ModifiedBy = _claimsService.GetCurrentUserId.ToString();
            _dbContext.Entry(student).State = EntityState.Modified;
            // await _dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> GetStudentWithScoreAndClassWithTrainingProgramAsync(int studentId, int classId)
        {
            return await _dbContext.Students.Include(s => s.StudentClasses.Where(sc => sc.StudentId == studentId && sc.ClassId == classId))
                .ThenInclude(sc => sc.Class)
                .ThenInclude(c => c.Program)
                .Include(s => s.Scores.Where(s => s.StudentId == studentId && s.ClassId == classId))
                .FirstOrDefaultAsync(x => x.Id == studentId);
        }



    }
}
