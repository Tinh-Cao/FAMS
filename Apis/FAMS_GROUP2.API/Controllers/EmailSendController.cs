using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.EmailSendsModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/email-sends")]
    [ApiController]
    public class EmailSendController : ControllerBase
    {
        private readonly IEmailSendService _emailSendServices;

        public EmailSendController(IEmailSendService emailSendService)
        {
            _emailSendServices = emailSendService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetEmailSendsByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] EmailSendsFilterModule emailSendsFilterModule)
        {
            try
            {
                var result = await _emailSendServices.GetAllEmailSendFilterBySendDateAsync(paginationParameter, emailSendsFilterModule);

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
        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetEmailSendByIdAsync([FromRoute] int id)
        {
            try
            {
                var eTemplate = await _emailSendServices.GetSendMailByIdAsync(id);
                return eTemplate == null ? NotFound() : Ok(eTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
