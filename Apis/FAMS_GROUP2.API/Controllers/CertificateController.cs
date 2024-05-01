using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/certificates")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificate([FromForm] CertificateModel certificateCreateModel)
        {
            try
            {
                var certificate = await _certificateService.CreateCertificateAsync(certificateCreateModel);
                if (certificate.Status == true)
                {
                    return Ok(certificate);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("certificate-template")]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetAllCertificateTemplate()
        {
            try
            {
                var certificates = await _certificateService.GetAllCertificateTemplate();
                if (certificates == null)
                {
                    return BadRequest("No certificates found for the student!");
                }
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("provide")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> ProvideCertificate([FromBody]CertificateProvideModel provideModels)
        {
            try
            {
                var certificate = await _certificateService.ProvideCertificateAsync(provideModels);
                if (certificate.Status == false)
                {
                    return BadRequest(certificate);
                }
                return Ok(certificate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("{studentId}/{classId}")]
        [Authorize(Roles = "SuperAdmin,Admin,Student")]
        public async Task<IActionResult> GetCertificate(int studentId, int classId)
        {
            try
            {
                var certificate = await _certificateService.GetCertificateAsync(studentId, classId);
                if (certificate == null)
                {
                    return BadRequest("No certificates found for the student in class!") ;
                }
                return Ok(certificate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetAllStudentCertificates([FromQuery] int studentId)
        {
            try
            {
                var certificates = await _certificateService.GetAllStudentCertificateAsync(studentId);
                if (certificates == null)
                {
                    return BadRequest("No certificates found for the student!");
                }
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{certificateId}")]
        //[Authorize(Roles = "SuperAdmin,Trainer,Admin")]
        public async Task<IActionResult> UpdateCertificateTemplate(int certificateId, [FromForm] CertificateModel certificateUpdateModel)
        {
            try
            {
                var result = await _certificateService.UpdateCertificateAsync(certificateId, certificateUpdateModel);
                if (result.Status == false)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete()]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> BlockCertificateTemplate([FromQuery] int certificateId)
        {
            try
            {
                var result = await _certificateService.DeleteCertificateAsync(certificateId);
                if (result.Status == false)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("unblock")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UnBlockCertificateTempalte([FromQuery] int certificateId)
        {
            try
            {
                var result = await _certificateService.UnDeleteCertificateAsync(certificateId);
                if (result.Status == false)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
