using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.TokenModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/authens")]
    [ApiController]
    public class AuthensController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthensController(IAccountService accountService) 
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(AccountRegisterModel account, [FromQuery] RoleEnums role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _accountService.RegisterAsync(account, role);
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

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AccountLoginModel account)
        {
            try
            {
                var result = await _accountService.LoginAsync(account);
                if (result.Status)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login-with-google")]
        public async Task<IActionResult> LoginGoogleAsync([FromBody] string credential)
        {
            try
            {
                var result = await _accountService.LoginGoogleAsync(credential);
                if (result.Status)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(JwtTokenModel token)
        {
            try
            {
                var result = await _accountService.RefreshToken(token);
                if (result.Status)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            try
            {
                var result = await _accountService.ChangePasswordAsync(changePassword);
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

        // Test
        [HttpGet("test-super-admin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> TestSuperAdmin()
        {
            return Ok("You can enter Super Admin resource");
        }

        [HttpGet("test-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TestAdmin()
        {
            return Ok("You can enter Admin resource");
        }
    }
}
