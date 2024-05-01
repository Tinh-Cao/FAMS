using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FluentAssertions;
using Moq;

namespace WebAPI.Tests.Repositories
{
    public class AccountRepositoryTests : SetupTest
    {
        private readonly IAccountRepository _accountRepository;


        public AccountRepositoryTests()
        {

            _accountRepository = new AccountRepository(_dbContext,
                _userManagerMock.Object, _configurationMock.Object,
                _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task AddRangeAccountsAsync_Should_ReturnCorrectData()
        {
            //ARRANGE
            var mockData = new List<AccountAddRangeModel>();
            var mockRoleName = "ExistingRole";
            var mockRole = _fixture.Build<Role>()
               .With(x => x.RoleName, mockRoleName)
               .Without(x => x.Account)
               .Create();
            await _dbContext.Roles.AddAsync(mockRole);
            var check = await _dbContext.SaveChangesAsync();
            var mockAccounts = _fixture.Build<Account>()
                                    .Without(a => a.ClassAccountAdmins)
                                    .Without(a => a.ClassAccountTrainers)
                                    .Without(a => a.EmailSends)
                                    .With(a => a.Role, mockRole)
                                    .CreateMany(4).ToList();

            _userManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            foreach (var account in mockAccounts)
            {
                mockData.Add(
                    new AccountAddRangeModel
                    {
                        Account = account,
                        rolename = mockRoleName
                    }
                    );
            }
            await _accountRepository.AddRangeAccountAsync(mockData);

            var paginationParameter = new PaginationParameter();
            var accountFilterModel = new AccountFilterModel();
            // act
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _accountRepository.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel);
            // assert
            saveChanges.Should().Be(mockData.Count());
            result.Should().HaveCount(mockData.Count());
            result.Should().BeEquivalentTo(mockData, options => options.WithoutStrictOrdering().ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetAccountDetails_ExistingAssdfsdfccount_ShouldReturnCorrectData()
        {
            //ARRANGE
            var mockData = new List<AccountAddRangeModel>();
            var mockRoleName = "ExistingRole";
            var mockRole = _fixture.Build<Role>()
               .With(x => x.RoleName, mockRoleName)
               .Without(x => x.Account) // dính circular reference
               .Create();
            await _dbContext.Roles.AddAsync(mockRole);
            var check = await _dbContext.SaveChangesAsync();
            var mockAccounts = _fixture.Build<Account>()
                                    .Without(a => a.ClassAccountAdmins)
                                    .Without(a => a.ClassAccountTrainers)
                                    .Without(a => a.EmailSends)
                                    .With(a => a.Role, mockRole)
                                    .CreateMany(1).ToList();

            _userManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            mockData.Add(
                new AccountAddRangeModel
                {
                    Account = mockAccounts.First(),
                    rolename = mockRoleName
                });

            // act
            var addedAccount = await _accountRepository.AddRangeAccountAsync(mockData);
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _accountRepository.GetAccountDetailsAsync(mockAccounts.First().Id);
            // Assert
            addedAccount.Should().BeTrue();
            result.Should().BeEquivalentTo(mockData.First(), options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task DeleteRangeExistingAccounts_ShouldReturnCorrectData()
        {
            //ARRANGE
            var mockData = new List<AccountAddRangeModel>();
            var mockRoleName = "ExistingRole";
            var mockRole = _fixture.Build<Role>()
               .With(x => x.RoleName, mockRoleName)
               .Without(x => x.Account)
               .Create();
            await _dbContext.Roles.AddAsync(mockRole);
            var lmao = await _dbContext.SaveChangesAsync();
            var mockAccounts = _fixture.Build<Account>()
                                    .Without(a => a.ClassAccountAdmins)
                                    .Without(a => a.ClassAccountTrainers)
                                    .Without(a => a.EmailSends)
                                    .With(a => a.Role, mockRole)
                                    .With(a => a.IsDelete, false)
                                    .CreateMany(3).ToList();

            _userManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            foreach (var account in mockAccounts)
            {
                mockData.Add(
                    new AccountAddRangeModel
                    {
                        Account = account,
                        rolename = mockRoleName
                    }
                    );
            }
            await _accountRepository.AddRangeAccountAsync(mockData);
            // Act
            var check = await _dbContext.SaveChangesAsync();
            var result = _accountRepository.DeleteRangeAccountAsync(mockAccounts);
            var saveChanges = await _dbContext.SaveChangesAsync();
            // Assert
            result.Should().AllSatisfy(b => b.IsDelete.Should().BeTrue());
            saveChanges.Should().Be(3);
        }

        [Fact]
        public async Task GetAccountByFilters_ShouldReturnCorrectData()
        {
            //ARRANGE
            var mockData = new List<AccountAddRangeModel>();
            var mockRoleName = "ExistingRole";
            var mockRole = _fixture.Build<Role>()
               .With(x => x.RoleName, mockRoleName)
               .Without(x => x.Account)
               .Create();
            await _dbContext.Roles.AddAsync(mockRole);
            var check = await _dbContext.SaveChangesAsync();
            var mockAccounts = _fixture.Build<Account>()
                                    .Without(a => a.ClassAccountAdmins)
                                    .Without(a => a.ClassAccountTrainers)
                                    .Without(a => a.EmailSends)
                                    .With(a => a.Role, mockRole)
                                    .CreateMany(4).ToList();

            _userManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            foreach (var account in mockAccounts)
            {
                mockData.Add(
                    new AccountAddRangeModel
                    {
                        Account = account,
                        rolename = mockRoleName
                    }
                    );
            }
            await _accountRepository.AddRangeAccountAsync(mockData);

            var paginationParameter = new PaginationParameter();
            var accountFilterModel = new AccountFilterModel();
            // act
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _accountRepository.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel);
            // assert
            saveChanges.Should().Be(mockData.Count());
            result.Should().HaveCount(mockData.Count());
            result.Should().BeEquivalentTo(mockData, options => options.WithoutStrictOrdering().ExcludingMissingMembers());
        }

        [Fact]
        public async Task UnbanRangeExistingAccount_ShouldReturnCorrectData()
        {
            //ARRANGE
            var mockData = new List<AccountAddRangeModel>();
            var mockRoleName = "ExistingRole";
            var mockRole = _fixture.Build<Role>()
               .With(x => x.RoleName, mockRoleName)
               .Without(x => x.Account)
               .Create();
            await _dbContext.Roles.AddAsync(mockRole);
            var lmao = await _dbContext.SaveChangesAsync();
            var mockAccounts = _fixture.Build<Account>()
                                    .Without(a => a.ClassAccountAdmins)
                                    .Without(a => a.ClassAccountTrainers)
                                    .Without(a => a.EmailSends)
                                    .With(a => a.Role, mockRole)
                                    .With(a => a.IsDelete, true)
                                    .CreateMany(1).ToList();

            _userManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            mockData.Add(
                new AccountAddRangeModel
                {
                    Account = mockAccounts.First(),
                    rolename = mockRoleName
                }
                );
            await _accountRepository.AddRangeAccountAsync(mockData);
            await _dbContext.SaveChangesAsync();
            // Act
            mockAccounts.ForEach(account => account.IsDelete = false);
            var result = await _accountRepository.UpdateAccountAsync(mockAccounts.First());
            var check = await _dbContext.SaveChangesAsync();
            var accountCheck = await _accountRepository.GetAccountDetailsAsync(mockAccounts.First().Id);

            // Assert
            check.Should().Be(1);
            Assert.True(accountCheck.IsDelete == false);
        }
    }
}
