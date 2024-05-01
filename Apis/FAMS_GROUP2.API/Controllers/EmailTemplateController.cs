using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/email-templates")]
    [ApiController]
    public class EmailTemplateController : ControllerBase
    {

        private readonly IEmailTemplateServices _emailTemplateServices;

        public EmailTemplateController(IEmailTemplateServices templateServices)
        {
            _emailTemplateServices = templateServices;
        }

        [HttpGet()]
        //[Authorize(Roles = "SuperAdmin")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAlllEmailTemplate()
        {
            try
            {
                return Ok(await _emailTemplateServices.GetAllEmailTemplateAsync());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        //[Authorize(Roles= "SuperAdmin")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetEmailTemplateByIdAsync([FromRoute] int id)
        {
            try
            {
                var eTemplate = await _emailTemplateServices.GetAllEmailTemplateByIdAsync(id);
                return eTemplate == null ? NotFound() : Ok(eTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost()]
        //[Authorize(Roles = "SuperAdmin")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<EmailTemplateResponseModel>> PostEmailTemplate([FromForm]EmailTemplateModel emailTemplate)
        {
            try
            {
                var newTemplate = await _emailTemplateServices.AddEmailTemplateAsync(emailTemplate);

                return Ok(newTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<EmailTemplateResponseModel>> UpdateEmailTemplate(int id, [FromBody] EmailTemplateModel model)
        {
            try
            {
                var updatedEmailTemplate = await _emailTemplateServices.UpdateEmailTemplateAsync(id, model);

                if (updatedEmailTemplate == null)
                {
                    // Handle the case where the email template with the given ID is not found
                    return NotFound();
                }

                // You can return the updated email template or a response model as needed
                

                return Ok(updatedEmailTemplate);
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("block/{id}")]
        public async Task<ActionResult<EmailTemplateResponseModel>> BanEmailTemplate(int id)
        {
            try 
            {
                var statusEmailTemplate = await _emailTemplateServices.BanEmailTemplateAsync(id);
                if (statusEmailTemplate == null)
                {
                    // Handle the case where the email template with the given ID is not found
                    return NotFound();
                }
                

                return Ok(statusEmailTemplate);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("unblock/{id}")]
        public async Task<ActionResult<EmailTemplateResponseModel>> UnBanEmailTemplate(int id)
        {
            try
            {
                var statusEmailTemplate = await _emailTemplateServices.UnBanEmailTemplateAsync(id);
                if (statusEmailTemplate == null)
                {
                    // Handle the case where the email template with the given ID is not found
                    return NotFound();
                }


                return Ok(statusEmailTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteEmailTemplate(int id)
        {
            try
            {
                var result = await _emailTemplateServices.DeleteEmailTemplateAsync(id);
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
    }

    
}
