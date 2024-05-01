using Application.ViewModels.ResponseModels;
using AutoFixture;
using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Routing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class DocumentControllerTests : SetupTest
    {
        private readonly DocumentController _documentController;
        private readonly Fixture _fixture;
        public DocumentControllerTests()
        {
            _documentController = new DocumentController(_documentServiceMock.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task CreateDocument_ShouldReturnOk_WhenDocumentAddedSucessfully()
        {
            // Arrange
            var documentViewModel = _fixture.Create<CreateDocumentModel>();
            _documentServiceMock.Setup(x => x.CreateDocumentAsync(documentViewModel))
                                .ReturnsAsync(new ResponseModel { Status = true, Message = "Added Document successfully" });
            // Act
            var result = await _documentController.CreateDocument(documentViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
            Assert.Equal("Added Document successfully", response.Message);
        }
        [Fact]
        public async Task CreateDocument_ShouldReturnBadRequest_WhenErrorAddingDocument()
        {
            // Arrange
            var documentViewModel = _fixture.Create<CreateDocumentModel>();
            _documentServiceMock.Setup(x => x.CreateDocumentAsync(documentViewModel))
                                .ReturnsAsync(new ResponseModel { Status = false, Message = "Error added!!" });
            // Act
            var result = await _documentController.CreateDocument(documentViewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(badRequestResult.Value);
            Assert.False(response.Status);
            Assert.Equal("Error added!!", response.Message);
        }
        [Fact]
        public async Task GetAllDocumentAsync_ShouldReturnListOfDocumentDetailsModel()
        {
            // Arrange
            var mockDocumentDetails = _fixture.Build<Document>().Without(a => a.Lesson).CreateMany(3).ToList();
            _documentServiceMock.Setup(x => x.GetAllDocumentAsync()).ReturnsAsync(mockDocumentDetails);
            // Act
            var result = await _documentController.GetAllDocument();

            // Assert          
            var okResult = Assert.IsType<OkObjectResult>(result);
            var documentDetails = (List<Document>)okResult.Value;
            Assert.Equal(mockDocumentDetails.Count, documentDetails.Count);
        }
        [Fact]
        public async Task GetAllDocumentAsync_ShouldReturnNotFound_WhenNoDocumentFound()
        {
            // Arrange
            _documentServiceMock.Setup(x => x.GetAllDocumentAsync()).ReturnsAsync((List<Document>)null);
            // Act
            var result = await _documentController.GetAllDocument();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task GetDocumentById_ShouldReturnDocumentDetails_WhenDocumentExists()
        {
            // Arrange
            var documentId = 1;
            var expectedDocument = _fixture.Build<Document>().Without(a => a.Lesson).Create();
            _documentServiceMock.Setup(x => x.GetDocumentByIdAsync(documentId)).ReturnsAsync(expectedDocument);
            // Act
            var result = await _documentController.GetDocumentById(documentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var DocumentDetails = Assert.IsType<Document>(okResult.Value);
            Assert.Equal(expectedDocument, DocumentDetails);
        }
        [Fact]
        public async Task GetDocumentById_ShouldReturnNotFound_WhenDocumentNotExists()
        {
            var documentId = 1;
            _documentServiceMock.Setup(x => x.GetDocumentByIdAsync(documentId)).ReturnsAsync((Document)null);
            // Act
            var result = await _documentController.GetDocumentById(documentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task UpdateDocument_ShouldReturnOk_WhenDocumentUpdateSuccessfully()
        {
            // Arrange
            var documentId = 1;
            var updateDocumentModel = _fixture.Create<UpdateDocumentModel>();
            _documentServiceMock.Setup(x => x.UpdateDocumentAsync(documentId, updateDocumentModel))
                                .ReturnsAsync(new ResponseModel { Status = true, Message = "Document update Successfully!" });
            // Act
            var result = await _documentController.UpdateDocument(documentId, updateDocumentModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
            Assert.Equal("Document update Successfully!", response.Message);
        }
        [Fact]
        public async Task UpdateDocument_ShouldReturnNotFound_WhenDocumentNotFound()
        {
            // Arrange 
            var documentId = 1;
            var updateDocumentModel = _fixture.Create<UpdateDocumentModel>();
            _documentServiceMock.Setup(x => x.UpdateDocumentAsync(documentId, updateDocumentModel))
                                .ReturnsAsync(new ResponseModel { Status = false, Message = "Document does not exists" });
            // Act
            var result = await _documentController.UpdateDocument(documentId, updateDocumentModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(notFoundResult.Value);
            Assert.False(response.Status);
            Assert.Equal("Document does not exists", response.Message);
        }
        [Fact]
        public async Task DeleteDocument_ShouldReturnSuccess()
        {
            // Arrange
            var documentId = 1;
            _documentServiceMock.Setup(x => x.DeleteDocumentAsync(documentId))
                                .ReturnsAsync(new ResponseModel { Status = true, Message = "Document deleted successfully" });
            // Act
            var result = await _documentController.DeleteDocument(documentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task DeleteDocument_ShouldReturnNotFound_WhenDocumentNotFound()
        {
            // Arrange
            var documentId = 1;
            _documentServiceMock.Setup(x => x.DeleteDocumentAsync(documentId))
                                .ReturnsAsync(new ResponseModel { Status = false, Message = "Document does not exists" });
            // Act
            var result = await _documentController.DeleteDocument(documentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task GetDocumentFilters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = _fixture.Create<PaginationParameter>();
            var documentFilterModel = _fixture.Create<DocumentFilterModel>();
            var documents = _fixture.CreateMany<DocumentDetailsModel>(10).ToList();
            var expectedResult = new Pagination<DocumentDetailsModel>(documents, 10, 1, 1);
            _documentServiceMock.Setup(x => x.GetDocumentFilterAsync(paginationParameter, documentFilterModel))
                                .ReturnsAsync(expectedResult);
            var httpContext = new DefaultHttpContext();
            var response = new Mock<HttpResponse>();
            var headers = new HeaderDictionary();
            headers.Add("X-Pagination", "");
            response.SetupGet(r => r.Headers).Returns(headers);
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);
            // Act
            _documentController.ControllerContext = controllerContext;
            var result = await _documentController.GetDocumentFilter(paginationParameter, documentFilterModel);
            // Assert
            _documentServiceMock.Verify(x => x.GetDocumentFilterAsync(paginationParameter, documentFilterModel), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
