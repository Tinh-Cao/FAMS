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

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseLoginModel> LoginAsync(AccountLoginModel account);

        Task<ResponseLoginModel> LoginGoogleAsync(string credetial);

        Task<ResponseModel> RegisterAsync(AccountRegisterModel account, RoleEnums role);

        Task<ResponseLoginModel> RefreshToken(JwtTokenModel token);

        Task<ResponseModel> ChangePasswordAsync(ChangePasswordModel passwordModel);
        Task<AccountImportResponseModel> AddAccountImportExcel(List<AccountImportModel> accounts);

        Task<AccountDetailsModel> GetAccountDetailsAsync(int accountId);
        Task<Pagination<AccountDetailsModel>> GetAccountsByFiltersAsync(PaginationParameter paginationParameter, AccountFilterModel accountFilterModel);
        Task<ResponseModel> UpdateAccountAsync(int accountId, AccountUpdateModel accountUpdateModel);
        Task<ResponseModel> DeleteRangeAccountAsync(List<int> accountIds);
        Task<ResponseModel> UnDeleteAccountAsync(List<int> accountId);
    }
}   
