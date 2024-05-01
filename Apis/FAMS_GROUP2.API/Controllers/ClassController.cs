using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> CreateClass(CreateClassModel createClassModel)
        {
            try
            {
                var result = await _classService.CreateClassAsync(createClassModel);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("account")]
        // [Authorize(Roles = "Student, Trainer, Admin")]
        public async Task<IActionResult> GetAllByAccountId(
            [FromQuery] ClassesGetByAccountIdModel classesGetByAccountIdModel)
        {
            try
            {
                var result = await _classService.GetClassesByAccountIdAsync(classesGetByAccountIdModel);
                return Ok(new ResponseDataModel<IEnumerable<ClassItemModel>>
                {
                    Status = true,
                    Message = "Get all classes by student id successfully",
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetClassesByFilters([FromQuery] PaginationParameter paginationParameter,
            [FromQuery] ClassesFilterModel classesFilterModel)
        {
            try
            {
                var result = await _classService.GetClassesByFiltersAsync(paginationParameter, classesFilterModel);
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetClassDetails(int classId)
        {
            try
            {
                var result = await _classService.GetClassDetailsAsync(classId);
                if (result.Status)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{classId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> UpdateClass([FromBody] UpdateClassModel updateClassModel, int classId)
        {
            try
            {
                var result = await _classService.UpdateClass(updateClassModel, classId);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{classId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            try
            {
                var result = await _classService.DeleteClass(classId);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("students")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> AddStudentToClass([FromBody] StudentsClassModel model)
        {
            try
            {
                var result = await _classService.AddStudentToClass(model.studentIdList, model.classId);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("students")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> DeleteStudentFromClass(StudentsClassModel model)
        {
            try
            {
                var result = await _classService.DeleteStudentFromClass(model.studentIdList, model.classId);
                if (result.Status)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}