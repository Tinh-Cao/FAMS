using AutoFixture;
using FAMS_GROUP2.API.Controllers;
using Domain.Tests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using Application.ViewModels.ResponseModels;
using FluentAssertions;


namespace WebAPI.Tests.Controllers
{
    public class CertificateControllerTests: SetupTest
    {
        private readonly CertificateController _certificateController;
        private readonly Fixture _fixture;
        public CertificateControllerTests()
        {
            _certificateController = new CertificateController(_certificateServiceMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateCertificateTemplate_ShouldReturnSuccess()
        {
            // Arrange
            var mockModelRequest = _fixture.Create<CertificateModel>();
            _certificateServiceMock.Setup(x => x.CreateCertificateAsync(mockModelRequest)).ReturnsAsync(new ResponseModel { Status = true});
            // Act
            var result = await _certificateController.CreateCertificate(mockModelRequest);
            // Assert
            _certificateServiceMock.Verify(x => x.CreateCertificateAsync(mockModelRequest), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            Assert.True(okResult.StatusCode == 200);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
        }

        [Fact]
        public async Task GetAllCertificateTemplate_ShouldReturnSuccess()
        {
            // Arrange
            var certificateTemplate = _fixture.Build<CertificateModel>()
                .CreateMany(10)
                .ToList();
            _certificateServiceMock.Setup(x => x.GetAllCertificateTemplate()).ReturnsAsync(certificateTemplate);
            // Act
            var result = await _certificateController.GetAllCertificateTemplate();
            // Assert
            _certificateServiceMock.Verify(x => x.GetAllCertificateTemplate(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            Assert.True(okResult.StatusCode == 200);
            okResult.Value.Should().BeEquivalentTo(certificateTemplate);
        }
        [Fact]
        public async Task UpdateCertificateTemplate_ShouldReturnSuccess()
        {
            // Arrange
            int certificateId = 1;
            var certificateUpdateModel = _fixture.Create<CertificateModel>();
            var expectedResult = new ResponseModel { Status = true };
            _certificateServiceMock.Setup(x => x.UpdateCertificateAsync(certificateId, certificateUpdateModel)).ReturnsAsync(expectedResult);
            // Act
            var result = await _certificateController.UpdateCertificateTemplate(certificateId, certificateUpdateModel);
            // Assert
            _certificateServiceMock.Verify(x => x.UpdateCertificateAsync(certificateId, certificateUpdateModel), Times.Once);
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            Assert.True(okResult.StatusCode == 200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task UpdateCertificateTemplate_ShouldReturnNotFound()
        {
            // Arrange
            int certificateId = 1;
            var certificateUpdateModel = _fixture.Create<CertificateModel>();
            var expectedResult = new ResponseModel { Status = false };
            _certificateServiceMock.Setup(x => x.UpdateCertificateAsync(certificateId, certificateUpdateModel)).ReturnsAsync(expectedResult);
            // Act
            var result = await _certificateController.UpdateCertificateTemplate(certificateId, certificateUpdateModel);
            // Assert
            _certificateServiceMock.Verify(x => x.UpdateCertificateAsync(certificateId, certificateUpdateModel), Times.Once);
            Assert.NotNull(result);
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.True(notFoundResult.StatusCode == 404);
        }

        [Fact]
        public async Task BanCertificateTemplate_ShouldReturnSuccess()
        {
            // Arrange
            int certificateTemplateId = 1;
            _certificateServiceMock.Setup(x => x.DeleteCertificateAsync(certificateTemplateId)).ReturnsAsync(new ResponseModel { Status = true });
            //act
            var result = await _certificateController.BlockCertificateTemplate(certificateTemplateId);
            // Assert
            _certificateServiceMock.Verify(x => x.DeleteCertificateAsync(certificateTemplateId), Times.Once());
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>(); //Success
            var okObjectResult = (OkObjectResult)result;
            Assert.True(okObjectResult.StatusCode == 200);
        }

        [Fact]
        public async Task BanCertificateTemplate_NonExistingAccount_ShouldReturnNotFound()
        {
            // Arrange 
            int certificateTemplateId = 99;
            _certificateServiceMock.Setup(x => x.DeleteCertificateAsync(certificateTemplateId)).ReturnsAsync(new ResponseModel { Status = false });
            //act
            var result = await _certificateController.BlockCertificateTemplate(certificateTemplateId);
            // Assert
            _certificateServiceMock.Verify(x => x.DeleteCertificateAsync(certificateTemplateId), Times.Once());
            Assert.NotNull(result);
            result.Should().BeOfType<NotFoundObjectResult>(); //Success
            var NotFoundResult = (NotFoundObjectResult)result;
            Assert.True(NotFoundResult.StatusCode == 404);
            Assert.Equal(404, NotFoundResult.StatusCode);
        }

        [Fact]
        public async Task ProvideCertificate_ShouldReturnSuccess()
        {
            // Arrange ehe
            var provideModels = _fixture.Create<CertificateProvideModel>();
            var expectedResponse = new ResponseModel { Status = true };
            _certificateServiceMock.Setup(x => x.ProvideCertificateAsync(provideModels)).ReturnsAsync(expectedResponse);
            // Act
            var result = await _certificateController.ProvideCertificate(provideModels);
            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            Assert.True(okResult.StatusCode == 200);
        }

        [Fact]
        public async Task GetCertificate_ShouldReturnSuccess()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var expectedCertificate = _fixture.Create<CertificateViewModel>();
            _certificateServiceMock.Setup(x => x.GetCertificateAsync(studentId, classId)).ReturnsAsync(expectedCertificate);
            // Act
            var result = await _certificateController.GetCertificate(studentId, classId);
            // Assert
            _certificateServiceMock.Verify(x => x.GetCertificateAsync(studentId, classId), Times.Once);
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            Assert.True(okResult.StatusCode == 200);
            okResult.Value.Should().BeEquivalentTo(expectedCertificate);
        }

        [Fact]
        public async Task GetAllStudentCertificates_ShouldReturnSuccess()
        {
            // Arrange
            var studentId = 1;
            var expectedCertificates = _fixture.CreateMany<CertificateViewModel>(5).ToList();
            _certificateServiceMock.Setup(x => x.GetAllStudentCertificateAsync(studentId)).ReturnsAsync(expectedCertificates);
            // Act
            var result = await _certificateController.GetAllStudentCertificates(studentId);
            // Assert
            _certificateServiceMock.Verify(x => x.GetAllStudentCertificateAsync(studentId), Times.Once);
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            Assert.True(okResult.StatusCode == 200);
            okResult.Value.Should().BeEquivalentTo(expectedCertificates);
        }
    }
}
