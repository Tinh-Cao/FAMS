using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.TokenModels;
using FAMS_GROUP2.Services.Interfaces;

namespace FAMS_GROUP2.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public async Task<ResponseModel> RegisterAsync(AccountRegisterModel account, RoleEnums role)
        {
            if (role == RoleEnums.Student)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Can not create account with this role!"
                };
            }
            return await _unitOfWork.AccountRepository.AddAccount(account, role);
        }

        public async Task<ResponseLoginModel> LoginAsync(AccountLoginModel account)
        {
            return await _unitOfWork.AccountRepository.LoginAsync(account);
        }

        public async Task<ResponseLoginModel> RefreshToken(JwtTokenModel token)
        {
            return await _unitOfWork.AccountRepository.RefreshToken(token);
        }

        public async Task<ResponseLoginModel> LoginGoogleAsync(string credetial)
        {
            return await _unitOfWork.AccountRepository.LoginGoogleAsync(credetial);
        }

        public async Task<ResponseModel> ChangePasswordAsync(ChangePasswordModel passwordModel)
        {
            return await _unitOfWork.AccountRepository.ChangePasswordAsync(passwordModel);
        }

        public async Task<AccountImportResponseModel> AddAccountImportExcel(List<AccountImportModel> accounts)
        {
            var existingAccounts = new List<AccountImportModel>();
            var importList = new List<AccountAddRangeModel>();
            try
            {
                foreach (var account in accounts)
                {
                    RoleEnums accountRole = EnumHelper.ConvertToRoleEnum(account.Role);
                    var user = new Account
                    {
                        Email = account.Email,
                        PhoneNumber = account.PhoneNumber,
                        Address = account.Address,
                        Dob = DateTime.Parse(account.Dob),
                        FullName = account.FullName,
                        Gender = (account.Gender.ToLower() == "male")
                    };
                    importList.Add(new AccountAddRangeModel
                    {
                        Account = user,
                        rolename = accountRole.ToString()
                    });
                }
                var existingEmails = await _unitOfWork.AccountRepository.GetAllEmail();
                var accountsToAdd = importList.Where(model => !existingEmails.Contains(model.Account.Email)).ToList();
                var statusAdd = await _unitOfWork.AccountRepository.AddRangeAccountAsync(accountsToAdd);
                var result = await _unitOfWork.SaveChangeAsync();
                existingAccounts = accounts.Where(account => existingEmails.Contains(account.Email)).ToList();
                return new AccountImportResponseModel
                {
                    Status = true,
                    Message = existingAccounts.Count >0  ? "Accounts added successfully but some accounts are existed" : "Accounts added successfully.",
                    ExistingAccounts = existingAccounts
                };
            }
            catch (Exception ex)
            {
                return new AccountImportResponseModel
                {
                    Status = false,
                    Message = "Error: " + ex.Message,
                    ExistingAccounts = existingAccounts
                };
            }
        }


        public async Task<AccountDetailsModel> GetAccountDetailsAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountDetailsAsync(accountId);
            if (account != null)
            {
                var role = await _unitOfWork.AccountRepository.GetRole(accountId);
                string genderString = account.Gender == true ? "Male" : "Female";
                var accountDetails = _mapper.Map<AccountDetailsModel>(account);
                accountDetails.Role = role;
                accountDetails.Gender = genderString;
                return accountDetails;
            }
            return null;
        }

        public async Task<Pagination<AccountDetailsModel>> GetAccountsByFiltersAsync(PaginationParameter paginationParameter, AccountFilterModel accountFilterModel)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel);
            var mappedResult = new List<AccountDetailsModel>();
            var roleNames = await _unitOfWork.AccountRepository.GetAllRoles();
            if (accounts != null)
            {
                foreach (var model in accounts)
                {
                    var mappedModel = _mapper.Map<AccountDetailsModel>(model);
                    mappedModel.Gender = mappedModel.Gender = true ? "Male" : "Female";
                    mappedModel.Role = roleNames.FirstOrDefault(roleName => roleName == model.Role.RoleName);
                    mappedResult.Add(mappedModel);
                }
                return new Pagination<AccountDetailsModel>(mappedResult, accounts.TotalCount, accounts.CurrentPage, accounts.PageSize);
            }
            return null;
        }

        public async Task<ResponseModel> UpdateAccountAsync(int accountId, AccountUpdateModel accountUpdateModel)
        {
            var existingAccount = await _unitOfWork.AccountRepository.GetAccountDetailsAsync(accountId);
            if (existingAccount != null)
            {
                existingAccount = _mapper.Map(accountUpdateModel, existingAccount);
                existingAccount.RoleId = EnumHelper.ConvertToRoleId(accountUpdateModel.Role);
                var updatedAccount = await _unitOfWork.AccountRepository.UpdateAccountAsync(existingAccount);
                await _unitOfWork.SaveChangeAsync();
                var responseModel = new ResponseModel
                {
                    Status = true,
                    Message = "Account updated successfully",
                };
                return responseModel;
            }
            return new ResponseModel
            {
                Status = false,
                Message = "Account is not existed!"
            };
        }

        public async Task<ResponseModel> DeleteRangeAccountAsync(List<int> accountIds)
        {
            var list = new List<Account>();

            foreach (int accountId in accountIds)
            {
                var result = await _unitOfWork.AccountRepository.GetAccountDetailsAsync(accountId);
                if (result != null)
                {
                    list.Add(result);
                }
            }
            if (list.Count > 0)
            {
                _unitOfWork.AccountRepository.DeleteRangeAccountAsync(list);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel
                {
                    Status = true,
                    Message = "Account ban successfully!"
                };
            }
            return new ResponseModel
            {
                Status = false,
                Message = "Not found!"
            };
        }

        public async Task<ResponseModel> UnDeleteAccountAsync(List<int> accountIds)
        {
            foreach (int accountId in accountIds)
            {
                var account = await _unitOfWork.AccountRepository.GetAccountDetailsAsync(accountId);
                if (account != null)
                {
                    account.IsDelete = false;
                    await _unitOfWork.AccountRepository.UpdateAccountAsync(account);
                }
                else if (account == null)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "One of these account is not founded!"
                    };
                }
            }

            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel
            {
                Status = true,
                Message = "Account unbanned successfully!"
            };
        }
    }
}
