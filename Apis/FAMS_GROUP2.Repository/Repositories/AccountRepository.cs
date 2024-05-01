using Application.Utils;
using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Utils;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.TokenModels;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FamsDbContext _dbContext;
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;

        public AccountRepository(FamsDbContext dbContext,
            IUserManager userManager,
            IConfiguration configuration,
            ICurrentTime currentTime,
            IClaimsService claimsService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _configuration = configuration;
            _currentTime = currentTime;
            _claimsService = claimsService;
        }
        public async Task<ResponseModel> AddAccount(AccountRegisterModel account, RoleEnums role)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(account.Email);
                if (userExist != null)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "Account is already exist!"
                    };
                }
                var user = new Account
                {
                    Email = account.Email,
                    PhoneNumber = account.PhoneNumber,
                    Address = account.Address,
                    Dob = account.Dob,
                    FullName = account.FullName,
                    Gender = account.Gender,
                };

                if (role.Equals(RoleEnums.SuperAdmin) || role.Equals(RoleEnums.Admin) || role.Equals(RoleEnums.Trainer))
                {
                    var statusAdd = await _userManager.CreateAsync(user, account.Password);

                    if (!statusAdd)
                    {
                        throw new Exception("Fail to add a new account!");
                    }

                    if (!await _userManager.RoleExistsAsync(role.ToString()))
                    {
                        await _userManager.CreateRoleAsync(role.ToString());
                    }

                    if (await _userManager.RoleExistsAsync(role.ToString()))
                    {
                        await _userManager.AddToRoleAsync(user, role.ToString());
                    }

                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Your account is ready. Try to login now."
                    };
                }

                return new ResponseModel
                {
                    Status = true,
                    Message = "Error"
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Error: " + ex.Message
                };
            }
        }

        public async Task<ResponseModel> ChangePasswordAsync(ChangePasswordModel passwordModel)
        {
            var account = await _userManager.FindByEmailAsync(passwordModel.Email);
            if (account == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Account does not exist!"
                };
            }
            await _userManager.ChangePasswordAsync(account, passwordModel.CurrentPassword, passwordModel.NewPassword);
            return new ResponseModel
            {
                Status = true,
                Message = "Change password successfully."
            };
        }

        public async Task<ResponseLoginModel> LoginAsync(AccountLoginModel account)
        {
            var accountExist = await _userManager.FindByEmailAsync(account.Email);
            if (accountExist == null)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Account does not exist!",
                };
            }

            if (accountExist.IsDelete == true)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Account is banned!",
                };
            }

            var result = await _userManager.PasswordSignInAsync(account.Email, account.Password);

            if (result)
            {
                var role = await _userManager.GetRoleAsync(accountExist);

                if (role != (RoleEnums.SuperAdmin.ToString()))
                {
                    return new ResponseLoginModel
                    {
                        Status = false,
                        Message = "Account is not allowed login to the system.",
                    };
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, accountExist.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                //generate refresh token
                var refreshToken = TokenTools.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                accountExist.RefreshToken = refreshToken;
                accountExist.RefreshTokenExpiryTime = _currentTime.GetCurrentTime().AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateInternalAsync(accountExist);

                var token = GenerateJWTToken.CreateToken(authClaims, _configuration, DateTime.UtcNow);

                return new ResponseLoginModel
                {
                    Status = true,
                    Message = "Login successfully.",
                    JWT = new JwtSecurityTokenHandler().WriteToken(token),
                    Expired = token.ValidTo.ToLocalTime(),
                    JWTRefreshToken = refreshToken,
                };
            }
            else
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Incorrect password!",
                };
            }
        }

        public async Task<ResponseLoginModel> LoginGoogleAsync(string credetial)
        {
            string clientId = _configuration["GoogleCredential:ClientId"];

            if (string.IsNullOrEmpty(clientId))
            {
                throw new Exception("ClientId is null!");
            }

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { clientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credetial, settings);
            if (payload == null)
            {
                throw new Exception("Credential incorrect!");
            }

            var accountExist = await _userManager.FindByEmailAsync(payload.Email);

            if (accountExist.IsDelete == true)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Account is banned!",
                };
            }

            if (accountExist != null)
            {
                // update image
                if (payload.Picture != null)
                {
                    if (accountExist.Image == null)
                    {
                        accountExist.Image = payload.Picture;
                        await _userManager.UpdateInternalAsync(accountExist);
                    }
                    else
                    {
                        if (payload.Picture != accountExist.Image)
                        {
                            accountExist.Image = payload.Picture;
                            await _userManager.UpdateInternalAsync(accountExist);
                        }
                    }
                }

                var role = await _userManager.GetRoleAsync(accountExist);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, accountExist.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                var refreshToken = TokenTools.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                accountExist.RefreshToken = refreshToken;
                accountExist.RefreshTokenExpiryTime = _currentTime.GetCurrentTime().AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateInternalAsync(accountExist);

                var token = GenerateJWTToken.CreateToken(authClaims, _configuration, DateTime.UtcNow);

                return new ResponseLoginModel
                {
                    Status = true,
                    Message = "Login successfully.",
                    JWT = new JwtSecurityTokenHandler().WriteToken(token),
                    Expired = token.ValidTo.ToLocalTime(),
                    JWTRefreshToken = refreshToken,
                };
            }
            else
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Your account is not allowed login to the system!",
                };
            }


        }

        public async Task<ResponseLoginModel> RefreshToken(JwtTokenModel token)
        {
            if (token is null)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Token is null!"
                };
            }

            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var principal = TokenTools.GetPrincipalFromExpiredToken(accessToken, _configuration);
            if (principal == null)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Invalid access token or refresh token!"
                };
            }

            int accountId = int.Parse(principal.Identity.Name);

            var account = await _userManager.FindByIdAsync(accountId);

            if (account == null || account.RefreshToken != refreshToken || account.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Invalid access token or refresh token!"
                };
            }

            var newAccessToken = GenerateJWTToken.CreateToken(principal.Claims.ToList(), _configuration, DateTime.UtcNow);
            var newRefreshToken = TokenTools.GenerateRefreshToken();

            account.RefreshToken = newRefreshToken;
            await _userManager.UpdateInternalAsync(account);

            return new ResponseLoginModel
            {
                Status = true,
                Message = "Refresh Token successfully!",
                JWT = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                Expired = newAccessToken.ValidTo.ToLocalTime(),
                JWTRefreshToken = newRefreshToken
            };
        }
        public async Task<bool> AddAccountAsync(Account account, string roleName)
        {
            try
            {
                var statusAdd = await _userManager.CreateAsync(account, "Abc@12345");

                if (!statusAdd)
                {
                    return false;
                }

                if (!await _userManager.RoleExistsAsync(roleName))
                {
                    await _userManager.CreateRoleAsync(roleName);
                }

                if (await _userManager.RoleExistsAsync(roleName))
                {
                    await _userManager.AddToRoleAsync(account, roleName);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> AddRangeAccountAsync(List<AccountAddRangeModel> importLists)
        {
            try
            {
                if (importLists.Count == 0)
                {
                    return false;
                }
                var list = new List<Account>();
                foreach (var account in importLists)
                {
                    //var statusAdd = await _userManager.CreateAsync(account.Account, "Abc@12345");

                    var hashPassword = PasswordTools.HashPassword("Abc@12345");
                    if (account.Account.FullName != null || account.Account.Address != null)
                    {
                        account.Account.UnsignFullName = StringTools.ConvertToUnSign(account.Account.FullName);
                        account.Account.UnsignAddress = StringTools.ConvertToUnSign(account.Account.Address);
                        account.Account.CreatedDate = _currentTime.GetCurrentTime();
                        account.Account.CreatedBy = _claimsService.GetCurrentUserId.ToString();
                    }
                    account.Account.PasswordHash = hashPassword;

                    if (!await _userManager.RoleExistsAsync(account.rolename) && account.rolename != null)
                    {
                        Role newRole = new Role
                        {
                            RoleName = account.rolename,
                            CreatedBy = _claimsService.GetCurrentUserId.ToString(),
                            CreatedDate = _currentTime.GetCurrentTime()
                        };
                        _dbContext.Add(newRole);
                        _dbContext.SaveChanges();
                        //  await _context.SaveChangesAsync();
                        account.Account.RoleId = newRole.Id;
                    }

                    if (await _userManager.RoleExistsAsync(account.rolename))
                    {
                        var role = await _dbContext.Roles.SingleAsync(r => r.RoleName == account.rolename);
                        account.Account.RoleId = role.Id;

                    }

                    list.Add(account.Account);

                }

                var result = _dbContext.AddRangeAsync(list).Status;

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<Account> GetAccountDetailsAsync(int accountId)
        {

            var accounts = await _userManager.FindByIdAsync(accountId);
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public async Task<string> GetRole(int accountId)
        {
            var account = await _userManager.FindByIdAsync(accountId);
            var role = await _userManager.GetRoleAsync(account);
            return role;
        }
        public async Task<List<string>> GetAllRoles()
        {
           return _dbContext.Roles.Select(r => r.RoleName).ToList();
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            var accountExist = await _userManager.FindByEmailAsync(email);
            return accountExist != null;
        }

        public async Task<List<string>> GetAllEmail()
        {
            return _dbContext.Accounts.Select(a => a.Email).ToList();
        }


        public async Task<Pagination<Account>> GetAccountsByFiltersAsync(PaginationParameter paginationParameter, AccountFilterModel accountFilterModel)
        {
            var accountsQuery = _dbContext.Accounts.Include(a => a.Role).AsNoTracking().AsQueryable();
            accountsQuery = await ApplyFilterSortAndSearch(accountsQuery, accountFilterModel);
            if (accountsQuery != null)
            {
                var sortedQuery = ApplySorting(accountsQuery, accountFilterModel);
                var totalCount = await sortedQuery.CountAsync();

                var accountsPagination = await sortedQuery
                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                    .Take(paginationParameter.PageSize)
                    .ToListAsync();
                return new Pagination<Account>(accountsPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            }
            return null;
        }

        private async Task<IQueryable<Account>> ApplyFilterSortAndSearch(IQueryable<Account> query, AccountFilterModel accountFilterModel)
        {
            if (accountFilterModel == null)
            {
                return query;
            }

            if (accountFilterModel.isDelete == true)
            {
                query = query.Where(a => a.IsDelete == true);
            }
            else if (accountFilterModel.isDelete == false)
            {
                query = query.Where(a => a.IsDelete == false);
            }
            else
            {
                query = query.Where(a => a.IsDelete == true || a.IsDelete == false);
            }

            if (!string.IsNullOrEmpty(accountFilterModel.Gender))
            {
                bool isMale = accountFilterModel.Gender.ToLower() == "male";
                query = query.Where(a => a.Gender == isMale);
            }

            if (!string.IsNullOrEmpty(accountFilterModel.Role))
            {
                var accountsInRole = await _userManager.GetUserInRoleAsync(accountFilterModel.Role);

                if (accountsInRole != null)
                {
                    var userIdsInRole = accountsInRole.Select(u => u.Id);
                    query = query.Where(a => userIdsInRole.Contains(a.Id));
                }
                else
                {
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(accountFilterModel.Search))
            {
                query = query.Where(a =>
                    a.FullName.Contains(accountFilterModel.Search) ||
                    a.UnsignFullName.Contains(accountFilterModel.Search)
                );
            }
            return query;
        }

        private IQueryable<Account> ApplySorting(IQueryable<Account> query, AccountFilterModel accountFilterModel)
        {
            switch (accountFilterModel.Sort.ToLower())
            {
                case "fullname":
                    query = (accountFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.FullName) : query.OrderByDescending(a => a.FullName);
                    break;
                case "dob":
                    query = (accountFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Dob) : query.OrderByDescending(a => a.Dob);
                    break;
                default:
                    query = (accountFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                    break;
            }

            return query;
        }
        public async Task<Account> UpdateAccountAsync(Account account)
        {
            account.ModifiedDate = _currentTime.GetCurrentTime();
            account.ModifiedBy = _claimsService.GetCurrentUserId.ToString();
            account.UnsignFullName = StringTools.ConvertToUnSign(account.FullName);
            account.UnsignAddress = StringTools.ConvertToUnSign(account.Address);
            _dbContext.Entry(account).State = EntityState.Modified;
            return account;

        }

        public Account DeleteAccountAsync(Account account)
        {
            account.DeletedDate = _currentTime.GetCurrentTime();
            account.DeletedBy = _claimsService.GetCurrentUserId.ToString();
            _dbContext.Entry(account).State = EntityState.Modified;
            return account;
        }

        public List<Account> DeleteRangeAccountAsync(List<Account> accounts)
        {
            foreach (Account account in accounts)
            {
                account.IsDelete = true;
                account.DeletedDate = _currentTime.GetCurrentTime();
                account.DeletedBy = _claimsService.GetCurrentUserId.ToString();
                _dbContext.Entry(account).State = EntityState.Modified;
                // await _dbContext.SaveChangesAsync();
            }
            return accounts;
        }
    }
}



