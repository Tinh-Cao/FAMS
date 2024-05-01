using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        [HttpPost()]
        public async Task<IActionResult> ImportRangeStudent(List<StudentImportModel> student)
        {
            try
            {
                if (student == null || student.Count == 0)
                {
                    throw new Exception();
                }
                return Ok(await _studentService.AddRangeStudent(student));
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin,Student,Trainer")]
        [HttpGet]
        public async Task<IActionResult> GetStudentByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] StudentFilterModel accountFilterModel)
        {
            try
            {
                var result = await _studentService.GetStudentsByFiltersAsync(paginationParameter, accountFilterModel);

                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginGoogleAsync([FromBody] string credential)
        {
            try
            {
                var result = await _studentService.LoginGoogleStudentAsync(credential);
                if (result.Status)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


         [Authorize(Roles = "SuperAdmin,Admin,Trainer,Student")]
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetAccountDetails(int studentId)
        {
            try
            {
                var result = await _studentService.GetStudentDetailsAsync(studentId);
                if (result == null)
                {
                    return NotFound("Student is not existed in Data");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        [HttpPut("{studentId}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int studentId, [FromBody] StudentUpdateModel student)
        {
            var result = await _studentService.UpdateStudentAsync(studentId, student);

            if (result == null)
            {
                return NotFound(new ResponseGenericModel<string>(null, false, "Student is not founded"));
            }
            else
            {
                return Ok(new ResponseGenericModel<StudentDetailsModel>(result, true, "Updated Successfully"));
            }

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteStudent(List<int> studentId)
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(studentId);
                if (result == null)
                {
                    return NotFound(new ResponseGenericModel<string>(null, false, "Student is not founded"));
                }
                return Ok(new ResponseGenericModel<List<StudentDetailsModel>>(result, true, "The students have been deactivated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("unban")]
        public async Task<IActionResult> UnbanAccount(List<int> studentIds)
        {
            try
            {
                var result = await _studentService.UnBanStudentAsync(studentIds);
                if (result == null)
                {
                    return NotFound(new ResponseGenericModel<string>(null, false, "Student is not founded"));
                }
                return Ok(new ResponseGenericModel<List<StudentDetailsModel>>(result, true, "The students have been activated"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseGenericModel<string>(null, false, ex.Message));
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Student")]
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetStudentsFromSpecificClass([FromRoute] int classId)
        {
            try
            {
                var list = await _studentService.GetStudentBySpecificClass(classId);
                return Ok(new { studentList = list });
            }
            catch (Exception)
            {

                throw;
            }
        }



    }


}
