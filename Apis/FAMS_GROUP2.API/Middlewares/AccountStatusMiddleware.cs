
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FAMS_GROUP2.Repositories.Interfaces;
using System.Security.Claims;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Utils;
using Newtonsoft.Json;

namespace FAMS_GROUP2.API.Middlewares
{
    public class AccountStatusMiddleware : IMiddleware
    {
        private readonly ILogger<AccountStatusMiddleware> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountStatusMiddleware( ILogger<AccountStatusMiddleware> logger, IConfiguration configuration, IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var extractedId = AuthenTools.GetCurrentAccountId(identity);
            var getCurrentUserId = string.IsNullOrEmpty(extractedId) ? 0 : int.Parse(extractedId);

            if (getCurrentUserId != 0)
            {
                var userAccount = await _accountRepository.GetAccountDetailsAsync(getCurrentUserId);
                if (userAccount != null && userAccount.IsDelete == true)
                {
                    userAccount.RefreshToken = null;
                    userAccount.RefreshTokenExpiryTime = null;
                    await _accountRepository.UpdateAccountAsync(userAccount);
                    var response = new
                    {
                        isBlocking = true,
                        message = "Account was banned!"
                    };
                    var jsonResponse = JsonConvert.SerializeObject(response);
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResponse);
                   
                    return;
                }
            }
            await next(context);
        } 
    }
}
