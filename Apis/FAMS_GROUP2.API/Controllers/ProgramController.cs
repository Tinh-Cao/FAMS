using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/trainingprograms")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
            
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin" )]
        public async Task<IActionResult> CreateProgramAsync(ProgramModel program)
        {
            try
            {
                var result = await _programService.CreateProgramAsync(program);
                if (result == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        Status = false,
                        Message = "Training Program Code is already existed!!!"
                    });
                }
                if (result != null)
                {
                    return Ok(new ResponseModel
                    {
                        Status = true,
                        Message = "Add training program succesfully"
                    });
                }
                return BadRequest(new ResponseModel
                {
                    Status = false,
                    Message = "Fail !!"
                });


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    
        [HttpGet("{id}")]
       [Authorize]

        public async Task<IActionResult> GetProgramById(int id)
        {
            try
            {
                var result = await _programService.GetProgramByIdAsync(id);
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
        [HttpPut("update/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> UpdateProgram([FromRoute]int id, [FromBody]UpdateProgramModel model)
        {
            try
            {
                var result = await _programService.UpdateProgramAsync(id, model);
                if (result == 1)
                {
                    return Ok(new ResponseModel
                    {
                        Status = true,
                        Message = "Update successfully"

                    });
                }
                else return NotFound();
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("pause/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> PauseProgram([FromRoute]int id)
        {
            try
            {
                var result = await _programService.UpdateProgramStatusAsync(id);
                if( result == 1)
                {
                    return BadRequest(new ResponseModel
                    {
                        Status = false,
                        Message = "Can change status of this training program because there is still class using  this TP"

                    });
                }
                if( result ==2)
                {
                    return Ok(new ResponseModel
                    {
                        Status = true,
                        Message = "Status of this tp has changed to Stop"
                    });
                }
                return NotFound(); 
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> DeleteProgramAsync(int id)
        {
            try
            {
                var result = await _programService.DeleteProgramAsync(id);

                if(result == 1)
                {
                    return Ok(new ResponseModel
                    {
                        Status = true,
                        Message = "Delele successfully!!!"
                    });
                }
                return NotFound();

            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
   
        [HttpGet("getprogramsbyfilter")]
        [Authorize]
        public async Task<IActionResult> getProgramsAsync([FromQuery] PaginationParameter paginationParameter,[FromQuery] ProgramFilterModel programFilterModel)
        {
            try
            {
                var result = await _programService.GetProgramsByFiltersAsync(paginationParameter, programFilterModel);
                if (result == null || result.Count == 0)
                {
                    return Ok(result);
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
        [HttpPost("addmodule")]
        [Authorize(Roles ="Admin,SuperAdmin")]
        public async Task<IActionResult> AddModuleToProgramAsync(int programId, int moduleId)
        {
            var result = await _programService.AddModuleToProgramAsync(programId, moduleId);
            return result ? Ok(new ResponseModel { Status = true, Message = "Module added to program successfully!" })
                          : BadRequest(new ResponseModel { Status = false, Message = "Failed to add module to program!" });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPrograms()
        {
            try
            {
                var result = await _programService.GetAllProgramAsync();
                return Ok(result);
            }catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
    }
}
