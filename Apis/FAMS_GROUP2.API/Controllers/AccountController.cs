using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Controllers
{

    [Route("api/v1/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost()]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ImportAccounts([FromBody] List<AccountImportModel> accounts)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _accountService.AddAccountImportExcel(accounts);
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

        [HttpGet("{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountDetails(int accountId)
        {
            try
            {
                var result = await _accountService.GetAccountDetailsAsync(accountId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
       [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> GetAccountByFilters([FromQuery] PaginationParameter paginationParameter, [FromQuery] AccountFilterModel accountFilterModel)
        {
            try
            {
                var result = await _accountService.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel);
                if (result == null)
                {
                    return NotFound("No accounts found with the specified filters.");
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

        [HttpPut("{accountId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAccount([FromRoute] int accountId, [FromBody] AccountUpdateModel accountUpdateModel)
        {
            try
            {
                var result = await _accountService.UpdateAccountAsync(accountId, accountUpdateModel);
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
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteAccount( List<int> accountId)
        {
            try
            {
                var result = await _accountService.DeleteRangeAccountAsync(accountId);
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

        [HttpPut("unban")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UnbanAccount( List<int> accountId)
        {
            try
            {
                var result = await _accountService.UnDeleteAccountAsync(accountId);
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
