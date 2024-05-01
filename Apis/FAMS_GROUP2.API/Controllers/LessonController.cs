using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/lessons")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> CreateLesson(CreateLessonModel model)
        {
            var result = await _lessonService.CreateLessonAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLesson()
        {
            try
            {
                var result = await _lessonService.GetAllLessonAsync();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            try
            {
                var result = await _lessonService.GetLessonByIdAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> UpdateLessonAsync(int id, [FromBody] UpdateLessonModel lesson)
        {
            var response = await _lessonService.UpdateLessonAsync(id, lesson);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> DeleteLessonAsync(int id)
        {
            try
            {
                var result = await _lessonService.DeleteLessonAsync(id);
                if (result.Status == true)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetLessonByFilter ([FromQuery] PaginationParameter paginationParameter, [FromQuery] LessonFilterModel lessonFilterModel)
        {
            try
            {
                var result = await _lessonService.GetLessonByFilterAsync(paginationParameter, lessonFilterModel);

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

    }
}
