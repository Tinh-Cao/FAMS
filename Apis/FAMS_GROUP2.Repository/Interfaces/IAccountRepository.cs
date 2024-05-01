using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IAccountRepository 
    {
        Task<ResponseLoginModel> LoginAsync(AccountLoginModel account);
        Task<ResponseLoginModel> LoginGoogleAsync(string credetial);
        Task<ResponseModel> AddAccount(AccountRegisterModel account, RoleEnums role);
        Task<ResponseLoginModel> RefreshToken(JwtTokenModel token);
        Task<ResponseModel> ChangePasswordAsync(ChangePasswordModel passwordModel);
        Task<bool> AddAccountAsync(Account account, string roleName);
        Task<bool> IsEmailExistAsync(string email);
        Task<List<string>> GetAllEmail();
        Task<Account> GetAccountDetailsAsync(int accountId);
        Task<string> GetRole(int accountId);
        Task<Pagination<Account>> GetAccountsByFiltersAsync(PaginationParameter paginationParameter, AccountFilterModel accountFilterModel);
        Task<Account> UpdateAccountAsync(Account existingAccount);
        Account DeleteAccountAsync(Account existingAccount);
        Task<bool> AddRangeAccountAsync(List<AccountAddRangeModel> importLists);
        List<Account> DeleteRangeAccountAsync(List<Account> accounts);
        Task<List<string>> GetAllRoles();
    }
}
