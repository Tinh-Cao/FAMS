using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace WebAPI.Tests.Controllers
{
    public class AccountControllerTests : SetupTest
    {
        private readonly AccountController _accountController;
        private readonly Fixture _fixture;

        public AccountControllerTests()
        {

            _accountController = new AccountController(_accountServiceMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ImportAccounts_ShouldReturnSuccess()
        {
            // Arrange
            var mockModelRequest = _fixture.CreateMany<AccountImportModel>(10).ToList();
            var mockModelResponse = _fixture.Build<AccountImportResponseModel>().Create();
            _accountServiceMock.Setup(x => x.AddAccountImportExcel(mockModelRequest)).ReturnsAsync(mockModelResponse);
            // Act
            var result = await _accountController.ImportAccounts(mockModelRequest);
            // Assert
            _accountServiceMock.Verify(x => x.AddAccountImportExcel(mockModelRequest), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(mockModelResponse);
        }

        [Fact]
        public async Task GetAccountDetails_ExistingAccount_ShouldReturnOkWithAccountDetails()
        {
            // Arrange
            int accountId = 1;
            var accountDetails = _fixture.Create<AccountDetailsModel>();
            _accountServiceMock.Setup(x => x.GetAccountDetailsAsync(accountId)).ReturnsAsync(accountDetails);
            // Act
            var result = await _accountController.GetAccountDetails(accountId);
            // Assert
            _accountServiceMock.Verify(x => x.GetAccountDetailsAsync(accountId), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(accountDetails);
        }

        [Fact]
        public async Task GetAccountDetails_NonExistingAccount_ShouldReturnNotFound()
        {
            // Arrange
            int nonExistentAccountId = 999;
            _accountServiceMock.Setup(x => x.GetAccountDetailsAsync(nonExistentAccountId)).ReturnsAsync((AccountDetailsModel)null);
            // Act
            var result = await _accountController.GetAccountDetails(nonExistentAccountId);
            // Assert
            _accountServiceMock.Verify(x => x.GetAccountDetailsAsync(nonExistentAccountId), Times.Once);
            Assert.NotNull(result);
            result.Should().BeOfType<NotFoundResult>(); //Success
            var NotFoundResult = (NotFoundResult)result;
            Assert.True(NotFoundResult.StatusCode == 404);
            Assert.Equal(404, NotFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetAccountByFilters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = _fixture.Create<PaginationParameter>();
            var accountFilterModel = _fixture.Create<AccountFilterModel>();
            var accounts = _fixture.CreateMany<AccountDetailsModel>(10).ToList();
            var expectedResult = new Pagination<AccountDetailsModel>(accounts, 10, 1, 1);
            _accountServiceMock.Setup(x => x.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel))
                               .ReturnsAsync(expectedResult);
            var httpContext = new DefaultHttpContext();
            var response = new Mock<HttpResponse>();
            var headers = new HeaderDictionary(); // Initialize headers
            headers.Add("X-Pagination", ""); // Add X-Pagination header
            response.SetupGet(r => r.Headers).Returns(headers); // Set the value for "X-Pagination"
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);
            // Act
            _accountController.ControllerContext = controllerContext;
            var result = await _accountController.GetAccountByFilters(paginationParameter, accountFilterModel);
            // Assert
            _accountServiceMock.Verify(x => x.GetAccountsByFiltersAsync(paginationParameter, accountFilterModel), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnSuccess()
        {
            // Arrange
            int accountId = 1;
            var accountUpdateModel = _fixture.Create<AccountUpdateModel>();
            var responseAccountMock =  _fixture.Build<Account>()
                                               .With(a => a.ClassAccountAdmins, new List<ClassAccount>())
                                               .With(a => a.ClassAccountTrainers, new List<ClassAccount>())
                                               .Without(a => a.EmailSends)
                                               .With(a => a.Role, new Role())
                                               .Create();
            _accountServiceMock.Setup(x => x.UpdateAccountAsync(accountId, accountUpdateModel))
                               .ReturnsAsync(new ResponseModel { Status = true });
            // Act
            var result = await _accountController.UpdateAccount(accountId, accountUpdateModel);
            // Assert
            _accountServiceMock.Verify(x => x.UpdateAccountAsync(accountId, accountUpdateModel), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            okResult.Value.Should().NotBeEquivalentTo(responseAccountMock);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
        }


        [Fact]
        public async Task UpdateAccount_ShouldReturnNotFound_WhenIdNotFound()
        {
            // Arrange
            int accountId = 99;
            var accountUpdateModel = _fixture.Create<AccountUpdateModel>();
            _accountServiceMock.Setup(x => x.UpdateAccountAsync(accountId, accountUpdateModel))
                               .ReturnsAsync(new ResponseModel { Status = false});
            // Act
            var result = await _accountController.UpdateAccount(accountId, accountUpdateModel);
            // Assert
            _accountServiceMock.Verify(x => x.UpdateAccountAsync(accountId, accountUpdateModel), Times.Once);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task DeleteAccounts_ShouldReturnSuccess()
        {
            // Arrange
            var accountId = _fixture.CreateMany<int>(3).ToList();
            var responseModel = new ResponseModel { Status = true};
            _accountServiceMock.Setup(x => x.DeleteRangeAccountAsync(accountId)).ReturnsAsync(responseModel);
            // Act
            var result = await _accountController.DeleteAccount(accountId);
            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(responseModel);
        }

        [Fact]
        public async Task DeleteAccounts_NonExistingAccount_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistentAccountIds = new List<int> { 99, 100, 101 };
            _accountServiceMock.Setup(x => x.DeleteRangeAccountAsync(nonExistentAccountIds)).ReturnsAsync(new ResponseModel { Status = false });
            // Act
            var result = await _accountController.DeleteAccount(nonExistentAccountIds);
            // Assert
            _accountServiceMock.Verify(x => x.DeleteRangeAccountAsync(nonExistentAccountIds), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UnbanAccount_ShouldReturnSuccess()
        {
            // Arrange
            var accountIds = _fixture.CreateMany<int>(3).ToList();
            var unbanResult = _fixture.Create<ResponseModel>();
            _accountServiceMock.Setup(x => x.UnDeleteAccountAsync(accountIds)).ReturnsAsync(unbanResult);
            // Act
            var result = await _accountController.UnbanAccount(accountIds);
            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(unbanResult);
        }

        [Fact]
        public async Task UnBanAccounts_NonExistingAccount_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistentAccountIds = new List<int> { 99, 100, 101 };
            _accountServiceMock.Setup(x => x.UnDeleteAccountAsync(nonExistentAccountIds)).ReturnsAsync(new ResponseModel { Status = false });
            // Act
            var result = await _accountController.UnbanAccount(nonExistentAccountIds);
            // Assert
            _accountServiceMock.Verify(x => x.UnDeleteAccountAsync(nonExistentAccountIds), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}

