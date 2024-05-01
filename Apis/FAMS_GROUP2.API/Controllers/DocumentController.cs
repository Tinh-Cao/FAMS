using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer")]
        public async Task<IActionResult> CreateDocument(CreateDocumentModel model)
        {
            var result = await _documentService.CreateDocumentAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDocument()
        {
            try
            {
                var result = await _documentService.GetAllDocumentAsync();
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
        public async Task<IActionResult> GetDocumentById(int id)
        {
            try
            {
                var result = await _documentService.GetDocumentByIdAsync(id);
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
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] UpdateDocumentModel document)
        {
            var response = await _documentService.UpdateDocumentAsync(id, document);

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
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                var result = await _documentService.DeleteDocumentAsync(id);
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
        public async Task<IActionResult> GetDocumentFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] DocumentFilterModel documentFilterModel)
        {
            try
            {
                var result = await _documentService.GetDocumentFilterAsync(paginationParameter, documentFilterModel);

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
