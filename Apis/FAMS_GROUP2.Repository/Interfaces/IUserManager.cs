using FAMS_GROUP2.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IUserManager
    {
        public Task<bool> CreateAsync(Account account, string accountPassword);

        public Task<Account> FindByEmailAsync(string email);

        public Task<bool> RoleExistsAsync(string role);

        public Task<bool> AddToRoleAsync(Account account, string role);

        public Task<bool> ChangePasswordAsync(Account account, string currentPassword, string newPassword);

        public Task<bool> PasswordSignInAsync(string email, string password);

        public Task<List<string>> GetRolesAsync1(Account account);

        public Task<string> GetRoleAsync(Account account);

        public Task<int> UpdateAsync(Account account);

        public Task<Account> FindByIdAsync(int accountId);

        public Task<int> CreateRoleAsync(string role);

        public Task<int> UpdateInternalAsync(Account account);

        public Task<List<Account>> GetUserInRoleAsync(string role);
    }
}
