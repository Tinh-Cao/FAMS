using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/modules")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModulesController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        [HttpPost]
        [Authorize(Roles ="SuperAdmin,Admin")]
        public async Task<IActionResult> CreateModuleAsync(CreateModuleViewModel model)
        {
            var result = await _moduleService.CreateModuleAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetAllModuleAsync()
        {
            try
            {
                var result = await _moduleService.GetAllModuleAsync();
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
        public async Task<IActionResult> GetModuleByID(int id)
        {
            try
            {
                var module = await _moduleService.GetModuleByIDAsync(id);
                if (module == null)
                {
                    return NotFound();
                }
                return Ok(module);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> UpdateModule([FromRoute] int id, [FromBody] UpdateModuleViewModel module)
        {
            var response = await _moduleService.UpdateModuleAsync(id, module);

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
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            try
            {
                var result = await _moduleService.DeleteModuleAsync(id);
                if (result.Status == false)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("pausemodule/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> PauseModule(int id)
        {
            try
            {
                var result = await _moduleService.PauseModuleAsync(id);
                if (result.Status == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetModuleByFilters([FromQuery] PaginationParameter paginationParameter, [FromQuery] ModuleFilterModule moduleFilterModule)
        {
            try
            {
                var result = await _moduleService.GetPaginationAsync(paginationParameter, moduleFilterModule);

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
