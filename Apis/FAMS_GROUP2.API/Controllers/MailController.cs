using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService ;
        public MailController(IMailService mailService)
        {
            _mailService = mailService ;    
        }
        [HttpPost]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
               
                return Ok(await _mailService.SendEmailAsync(request));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
