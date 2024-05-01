using Application.ViewModels.ResponseModels;
using AutoFixture;
using AutoMapper;
using Domain.Tests;
using FluentAssertions;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Moq;
using FAMS_GROUP2.Repositories.ViewModels.TokenModels;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Helper;

namespace WebAPI.Tests.Services
{
    public class AccountServiceTests : SetupTest
    {
        private readonly IAccountService _accountService;

        public AccountServiceTests()
        {
            _accountService = new AccountService(_mapperConfig, _unitOfWorkMock.Object);
        }


        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldReturnSuccessResponse()
        {
            // Arrange
            var account = _fixture.Build<AccountRegisterModel>().Create();
            var role = RoleEnums.SuperAdmin;
            var expectedResponse = new ResponseModel { Status = true };
            _unitOfWorkMock.Setup(u => u.AccountRepository.AddAccount(account, role)).ReturnsAsync(expectedResponse);
            // Act
            var result = await _accountService.RegisterAsync(account, role);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.AddAccount(account, role), Times.Once());
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task LoginAsync_WithValidData_ShouldReturnResponseLoginModel()
        {
            // Arrange
            var account = _fixture.Create<AccountLoginModel>();
            var expectedResponse = _fixture.Create<ResponseLoginModel>();
            _unitOfWorkMock.Setup(u => u.AccountRepository.LoginAsync(account))
                            .ReturnsAsync(expectedResponse);
            // Act
            var result = await _accountService.LoginAsync(account);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.LoginAsync(account), Times.Once());
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task RefreshToken_WithValidData_ShouldReturnResponseLoginModel()
        {
            // Arrange
            var token = _fixture.Create<JwtTokenModel>();
            var expectedResponse = _fixture.Create<ResponseLoginModel>();
            _unitOfWorkMock.Setup(u => u.AccountRepository.RefreshToken(token))
                            .ReturnsAsync(expectedResponse);
            // Act
            var result = await _accountService.RefreshToken(token);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.RefreshToken(token), Times.Once());
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task LoginGoogleAsync_WithValidCredential_ShouldReturnResponseLoginModel()
        {
            // Arrange
            var credential = _fixture.Create<string>();
            var expectedResponse = _fixture.Create<ResponseLoginModel>();
            _unitOfWorkMock.Setup(u => u.AccountRepository.LoginGoogleAsync(credential))
                            .ReturnsAsync(expectedResponse);
            // Act
            var result = await _accountService.LoginGoogleAsync(credential);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.LoginGoogleAsync(credential), Times.Once());
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task ChangePasswordAsync_WithValidData_ShouldReturnResponseModel()
        {
            // Arrange
            var passwordModel = _fixture.Create<ChangePasswordModel>();
            var expectedResponse = _fixture.Create<ResponseModel>();
            _unitOfWorkMock.Setup(u => u.AccountRepository.ChangePasswordAsync(passwordModel))
                            .ReturnsAsync(expectedResponse);
            // Act
            var result = await _accountService.ChangePasswordAsync(passwordModel);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.ChangePasswordAsync(passwordModel), Times.Once());
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task AddAccountImportExcel_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var accounts = _fixture.Build<AccountImportModel>()
                            .With(a => a.Role, "superadmin")
                            .With(a => a.Gender, "Male")
                            .With( a => a.Dob, "2024-03-27")
                            .CreateMany(2).ToList();

            var expectedResult = new AccountImportResponseModel
            {
                Status = true,
                Message = "Accounts added successfully.",
                ExistingAccounts = new List<AccountImportModel>()
            };            // Setup mock to return false for IsEmailExistAsync
            _unitOfWorkMock.Setup(u => u.AccountRepository.IsEmailExistAsync(It.IsAny<string>())).ReturnsAsync(false);
            // Setup Mock to return true AddRangeAccountAsync
            _unitOfWorkMock.Setup(u => u.AccountRepository.AddRangeAccountAsync(It.IsAny<List<AccountAddRangeModel>>()))
                .ReturnsAsync(true);
            // Setup mock for SaveChangeAsync
            int saveChangesCallCount = 0;
            _unitOfWorkMock.Setup(u => u.SaveChangeAsync())
                .ReturnsAsync(() => ++saveChangesCallCount);
            // Act
            var result = await _accountService.AddAccountImportExcel(accounts);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.IsEmailExistAsync(It.IsAny<string>()), Times.Exactly(accounts.Count));
            _unitOfWorkMock.Verify(u => u.AccountRepository.AddRangeAccountAsync(It.IsAny<List<AccountAddRangeModel>>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
            Assert.True(result.Status == expectedResult.Status);
        }


        [Fact]
        public async Task GetAccountDetailsAsync_WithValidAccountId_ShouldReturnAccountDetailsModel()
        {
            // Arrange
            var accountId = _fixture.Create<int>();
            var mockRoleName = "SuperAdmin";
            var expectedGender = "Female";
            var mockRole = _fixture.Build<Role>()
               .With(x => x.RoleName, mockRoleName)
               .Without(x => x.Account)
               .Create();
            var account = _fixture.Build<Account>().Without(a => a.ClassAccountAdmins)
                                                   .Without(a => a.ClassAccountTrainers)
                                                   .Without(a => a.EmailSends)
                                                   .With(a => a.Role, mockRole)
                                                   .With(a => a.Gender, false)
                                                   .Create();
            var expectedRusult = _mapperConfig.Map<AccountDetailsModel>(account);
            expectedRusult.Role = mockRoleName;
            expectedRusult.Gender = expectedGender;
            _unitOfWorkMock.Setup(u => u.AccountRepository.GetAccountDetailsAsync(accountId))
                            .ReturnsAsync(account);
            _unitOfWorkMock.Setup(u => u.AccountRepository.GetRole(accountId))
                            .ReturnsAsync(mockRoleName);
            // Act
            var result = await _accountService.GetAccountDetailsAsync(accountId);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.GetAccountDetailsAsync(accountId), Times.Once());
            _unitOfWorkMock.Verify(u => u.AccountRepository.GetRole(accountId), Times.Once());
            result.Should().BeEquivalentTo(expectedRusult);
        }

        [Fact]
        public async Task GetAccountsByFiltersAsync_WithValidParameters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = _fixture.Create<PaginationParameter>();
            var accountFilterModel = _fixture.Create<AccountFilterModel>();
            var mockRoleName = "SuperAdmin";
            var expectedGender = "Female";
            var mockRole = _fixture.Build<Role>()
                                   .With(x => x.RoleName, mockRoleName)
                                   .Without(x => x.Account)
                                   .Create();
            var accounts = _fixture.Build<Account>()
                                   .Without(a => a.ClassAccountAdmins)
                                   .Without(a => a.ClassAccountTrainers)
                                   .Without(a => a.EmailSends)
                                   .With(a => a.Role, mockRole)
                                   .With(a => a.Gender, false)
                                   .CreateMany().ToList();
            var totalAccounts = accounts.Count;
            var mappedAccounts = _mapperConfig.Map<List<AccountDetailsModel>>(accounts);

            // Set the Role property of AccountDetailsModel to the role name string
            foreach (var account in mappedAccounts)
            {
                account.Role = mockRoleName;
                account.Gender = expectedGender;
            }
            var expectedPagination = new Pagination<AccountDetailsModel>(mappedAccounts, totalAccounts, paginationParameter.PageIndex, paginationParameter.PageSize);

            _unitOfWorkMock.Setup(u => u.AccountRepository.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel))
                            .ReturnsAsync(new Pagination<Account>(accounts, totalAccounts, paginationParameter.PageIndex, paginationParameter.PageSize));
            foreach (var model in accounts)
            {
                _unitOfWorkMock.Setup(u => u.AccountRepository.GetRole(model.Id))
                                .ReturnsAsync(mockRoleName);
            }
            // Act
            var result = await _accountService.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel), Times.Once());
            foreach (var model in accounts)
            {
                _unitOfWorkMock.Verify(u => u.AccountRepository.GetRole(model.Id), Times.Once());
            }
            result.Should().BeEquivalentTo(expectedPagination);
        }

        [Fact]
        public async Task UpdateAccountAsync_WithExistingAccountId_ShouldReturnSuccessResponse()
        {
            // Arrange
            var accountId = _fixture.Create<int>();
            var mockRoleName = "SuperAdmin";
            var accountUpdateModel = _fixture.Build<AccountUpdateModel>()
                                     .With(a => a.Dob, DateTime.Now.ToString())
                                     .With(a => a.Role, "Admin")
                                     .Create();
            var expectedGender = "Female";
            var mockRole = _fixture.Build<Role>()
                                   .With(x => x.RoleName, mockRoleName)
                                   .Without(x => x.Account)
                                   .Create();
            var existingAccount = _fixture.Build<Account>()
                                   .Without(a => a.ClassAccountAdmins)
                                   .Without(a => a.ClassAccountTrainers)
                                   .Without(a => a.EmailSends)
                                   .With(a => a.Role, mockRole)
                                   .With(a => a.Gender, false)
                                   .Create();
            // Mocking the existing account retrieval
            _unitOfWorkMock.Setup(u => u.AccountRepository.GetAccountDetailsAsync(accountId))
                .ReturnsAsync(existingAccount);
            // Mocking the account update
            _unitOfWorkMock.Setup(u => u.AccountRepository.UpdateAccountAsync(It.IsAny<Account>()))
                .ReturnsAsync(existingAccount);
            // Act
            var result = await _accountService.UpdateAccountAsync(accountId, accountUpdateModel);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.GetAccountDetailsAsync(accountId), Times.Once());
            _unitOfWorkMock.Verify(u => u.AccountRepository.UpdateAccountAsync(It.IsAny<Account>()), Times.Once());
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once());
            // Check the response status
            result.Should().BeEquivalentTo(new ResponseModel { Status = true, Message = "Account updated successfully" });
        }


        [Fact]
        public async Task DeleteRangeAccountAsync_WithValidAccountIds_ShouldReturnSuccessResponse()
        {
            // Arrange
            var accountIds = _fixture.CreateMany<int>().ToList();
            var accounts = _fixture.Build<Account>()
                                   .Without(a => a.ClassAccountAdmins)
                                   .Without(a => a.ClassAccountTrainers)
                                   .Without(a => a.EmailSends)
                                   .Without(a => a.Role)
                                   .CreateMany().ToList();
            var deletedAccounts = accounts.Take(accountIds.Count).ToList();
            // Setup sequence to return each deleted account
            var sequence = _unitOfWorkMock.SetupSequence(u => u.AccountRepository.GetAccountDetailsAsync(It.IsAny<int>()));
            foreach (var account in deletedAccounts)
            {
                sequence = sequence.ReturnsAsync(account);
            }
            _unitOfWorkMock.Setup(u => u.AccountRepository.DeleteRangeAccountAsync(deletedAccounts))
                           .Returns(accounts);
            // Act
            var result = await _accountService.DeleteRangeAccountAsync(accountIds);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.GetAccountDetailsAsync(It.IsAny<int>()), Times.Exactly(accountIds.Count));
            _unitOfWorkMock.Verify(u => u.AccountRepository.DeleteRangeAccountAsync(deletedAccounts), Times.Once());
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once());
            result.Should().BeEquivalentTo(new ResponseModel { Status = true, Message = "Account ban successfully!" });
        }


        [Fact]
        public async Task UnDeleteAccountAsync_WithValidAccountIds_ShouldReturnSuccessResponse()
        {
            // Arrange
            var accountIds = _fixture.CreateMany<int>().ToList();
            var accounts = _fixture.Build<Account>()
                                   .Without(a => a.ClassAccountAdmins)
                                   .Without(a => a.ClassAccountTrainers)
                                   .Without(a => a.EmailSends)
                                   .Without(a => a.Role)
                                   .CreateMany().ToList();
            var existingAccounts = accounts.Take(accountIds.Count).ToList();
            foreach (var account in existingAccounts)
            {
                account.IsDelete = true;
            }
            var sequence = _unitOfWorkMock.SetupSequence(u => u.AccountRepository.GetAccountDetailsAsync(It.IsAny<int>()));
            foreach (var account in existingAccounts)
            {
                sequence = sequence.ReturnsAsync(account);
            }
            _unitOfWorkMock.Setup(u => u.AccountRepository.UpdateAccountAsync(It.IsAny<Account>()))
                            .Returns(Task.FromResult((Account)null));
            // Act
            var result = await _accountService.UnDeleteAccountAsync(accountIds);
            // Assert
            _unitOfWorkMock.Verify(u => u.AccountRepository.GetAccountDetailsAsync(It.IsAny<int>()), Times.Exactly(accountIds.Count));
            _unitOfWorkMock.Verify(u => u.AccountRepository.UpdateAccountAsync(It.IsAny<Account>()), Times.Exactly(accountIds.Count));
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once());
            result.Should().BeEquivalentTo(new ResponseModel { Status = true, Message = "Account unbanned successfully!" });
        }


    }
}
