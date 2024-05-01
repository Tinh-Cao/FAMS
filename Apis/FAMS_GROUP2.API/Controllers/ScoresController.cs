using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/scores")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly IScoreService _scoreService;

        public ScoresController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpGet("class")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> GetScoresByFilters([FromQuery] PaginationParameter paginationParameter, [FromQuery] ScoreFilterModel scoreFilterModel)
        {
            try
            {
                var result = await _scoreService.GetScoresByFiltersAsync(paginationParameter, scoreFilterModel);
                if (result == null)
                {
                    return NotFound("No scores found with the specified filters.");
                }
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

        [HttpGet("student")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Student")]
        public async Task<IActionResult> GetScoresByStudentId([FromQuery] int studentId, [FromQuery] int classId)
        {
            try
            {
                var scores = await _scoreService.GetScoreByIdAsync(studentId, classId);
                if (scores != null)
                {
                    return Ok(scores);
                }
                else
                {
                    return Ok(new ResponseDataModel<Score>
                    {
                        Status = false,
                        Message = "This Student doesn't have scores",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("student")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> AddScoresByForm([FromBody] ScoreCreateModel scoreModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _scoreService.AddScoreByFormAsync(scoreModel);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("student")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> UpdateScoresByStudent([FromQuery] int StudentId, [FromQuery] int ClassId, [FromBody] ScoreUpdateModel scoreModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _scoreService.UpdateScoreByStudentAsync(StudentId, ClassId, scoreModel);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("class")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> UpdateScoresByClass([FromBody] List<ScoreCreateModel> scoreModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _scoreService.UpdateScoreByClassAsync(scoreModel);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("student")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> DeleteScore([FromQuery] int StudentId, [FromQuery] int ClassId)
        {
            try
            {
                var result = await _scoreService.DeleteScoreAsync(StudentId, ClassId);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("class")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> DeleteScoresByClass([FromQuery] List<int> StudentId, [FromQuery] int ClassId)
        {
            try
            {
                var result = await _scoreService.DeleteScoresByClassAsync(StudentId, ClassId);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("class")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> ImportScore([FromBody] List<ScoreImportModel> scoreModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _scoreService.ScoreImportExcel(scoreModel);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}