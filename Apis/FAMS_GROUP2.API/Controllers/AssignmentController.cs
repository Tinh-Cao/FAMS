using System.ComponentModel.DataAnnotations;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/assignment")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsmsByFiltersAsync([FromQuery] PaginationParameter paginationParameter,
            [FromQuery] AssignmentFilterModel asmFilterModel)
        {
            try
            {
                var result = await _assignmentService.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel);

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
                return BadRequest(new ResponseModel
                {
                    Status = false,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost("{moduleId}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> CreateAsmByExcelAsync([Required] int moduleId,
            List<AssignmentImportModel> models)
        {
            try
            {
                var result = await _assignmentService.CreateAsmByExcelAsync(moduleId, models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> UpdateAsmAsync([Required] int id, AssignmentImportModel model)
        {
            try
            {
                var result = await _assignmentService.UpdateAsmAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetAsmDetail([Required] int id)
        {
            try
            {
                var result = await _assignmentService.GetAsmById(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> SoftDeleteById([Required] int id)
        {
            try
            {
                var result = await _assignmentService.SoftDeleteAsmById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}