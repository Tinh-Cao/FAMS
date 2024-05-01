using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class UserManager : IUserManager
    {
        private readonly FamsDbContext _context;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;

        public UserManager(FamsDbContext context,
            IClaimsService claimsService,
            ICurrentTime currentTime)
        {
            _context = context;
            _claimsService = claimsService;
            _currentTime = currentTime;
        }
        public async Task<bool> AddToRoleAsync(Account account, string role)
        {
            var roleName = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == role);
            if (roleName != null)
            {
                account.RoleId = roleName.Id;
                //_context.Update(account);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ChangePasswordAsync(Account account, string currentPassword, string newPassword)
        {
            var user = await FindByEmailAsync(account.Email);
            if (user == null)
            {
                throw new Exception("Not found account!");
            }
            var checkPassword = PasswordTools.VerifyPassword(currentPassword, user.PasswordHash);
            if (checkPassword)
            {
                user.PasswordHash = PasswordTools.HashPassword(newPassword);
                user.ModifiedDate = _currentTime.GetCurrentTime();
                user.ModifiedBy = _claimsService.GetCurrentUserId.ToString();
                _context.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new Exception("Password is incorrect!");
            }
        }

        public async Task<bool> CreateAsync(Account account, string accountPassword)
        {
            var hashPassword = PasswordTools.HashPassword(accountPassword);
            if (account.FullName != null || account.Address != null)
            {
                account.UnsignFullName = StringTools.ConvertToUnSign(account.FullName);
                account.UnsignAddress = StringTools.ConvertToUnSign(account.Address);
                account.CreatedDate = _currentTime.GetCurrentTime();
                account.CreatedBy = _claimsService.GetCurrentUserId.ToString();
            }
            account.PasswordHash = hashPassword;
            _context.Add(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateRoleAsync(string role)
        {
            Role newRole = new Role
            {
                RoleName = role,
                CreatedBy = _claimsService.GetCurrentUserId.ToString(),
                CreatedDate = _currentTime.GetCurrentTime()
            };
            _context.Add(newRole);
            await _context.SaveChangesAsync();
            return newRole.Id;
        }

        public async Task<Account> FindByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account> FindByIdAsync(int accountId)
        {
            return await _context.Accounts.SingleOrDefaultAsync(a => a.Id == accountId);
        }

        public async Task<List<string>> GetRolesAsync1(Account account)
        {
            var roles = await _context.Roles.Where(r => r.Id == account.RoleId).Select(r => r.RoleName).ToListAsync();
            return roles.Any() ? roles : null;
        }

        public async Task<string> GetRoleAsync(Account account)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == account.RoleId);
            return role == null ? null : role.RoleName;
        }

        public async Task<List<Account>> GetUserInRoleAsync(string role)
        {
            var roleExist = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == role);
            if (roleExist == null)
            {
                throw new Exception("Invalid Role");
            }
            var list = await _context.Accounts.Where(x => x.RoleId == roleExist.Id).ToListAsync();
            return list;
        }

        public async Task<bool> PasswordSignInAsync(string email, string password)
        {
            var user = await FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return PasswordTools.VerifyPassword(password, user.PasswordHash);
        }

        public async Task<bool> RoleExistsAsync(string role)
        {
            var roleExist = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == role);
            return true ? roleExist != null : false;
        }

        public async Task<int> UpdateAsync(Account account)
        {
            account.ModifiedDate = _currentTime.GetCurrentTime();
            account.ModifiedBy = _claimsService.GetCurrentUserId.ToString();
            _context.Update(account);
            await _context.SaveChangesAsync();
            return account.Id;
        }

        public async Task<int> UpdateInternalAsync(Account account)
        {
            _context.Update(account);
            await _context.SaveChangesAsync();
            return account.Id;
        }
    }
}
