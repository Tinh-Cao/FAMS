using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Services
{
    public class DocumentServiceTests : SetupTest
    {
        private readonly IDocumentService _documentService;
        private readonly Fixture _fixture;
        public DocumentServiceTests()
        {
            _documentService = new DocumentService(_mapperConfig, _unitOfWorkMock.Object);
            _fixture = new Fixture();   
        }
        [Fact]
        public async Task CreateDocumentAsync_Should_ReturnCorrectData()
        {
            // Arrange
            var documentModel = _fixture.Create<CreateDocumentModel>();

            var mockDocument = _mapperConfig.Map<Document>(documentModel);
            _unitOfWorkMock.Setup(a => a.DocumentRepository.AddAsync(It.IsAny<Document>()))
                           .ReturnsAsync(mockDocument);
            // Act
            var result = await _documentService.CreateDocumentAsync(documentModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Be("Add Document Successfully");
            _unitOfWorkMock.Verify(a => a.DocumentRepository.AddAsync(It.IsAny<Document>()), Times.Once());
            _unitOfWorkMock.Verify(a => a.SaveChangeAsync(), Times.Once());
        }
        [Fact]
        public async Task CreateDocumentAsync_Should_ReturnError_WhenAddDocumentFails()
        {
            // Arrange
            var documentModel = _fixture.Create<CreateDocumentModel>();

            _unitOfWorkMock.Setup(a => a.DocumentRepository.AddAsync(It.IsAny<Document>()))
                           .ReturnsAsync((Document)null);
            // Act
            var result = await _documentService.CreateDocumentAsync(documentModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            result.Message.Should().Be("Error added!!");
            _unitOfWorkMock.Verify(a => a.DocumentRepository.AddAsync(It.IsAny<Document>()), Times.Once());
        }
        [Fact]
        public async Task GetDocumentByIdAsync_Should_ReturnDocumentDetails_WhenDocumentExists()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            var documentId = 1;
            var mockDocument = _fixture.Create<Document>();
            _unitOfWorkMock.Setup(a => a.DocumentRepository.GetByIdAsync(documentId)).ReturnsAsync(mockDocument);
            var expectedDocumentDetails = _mapperConfig.Map<DocumentDetailsModel>(mockDocument);

            // Act
            var result = await _documentService.GetDocumentByIdAsync(documentId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDocumentDetails);
            _unitOfWorkMock.Verify(a => a.DocumentRepository.GetByIdAsync(documentId), Times.Once);
        }
        [Fact]
        public async Task GetDocumentByIdAsync_Should_ReturnNull_WhenDocumentDoesNotExist()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
           .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            var documentId = 1;
            _unitOfWorkMock.Setup(a => a.DocumentRepository.GetByIdAsync(documentId)).ReturnsAsync((Document)null);

            // Act
            var result = await _documentService.GetDocumentByIdAsync(documentId);

            // Assert
            result.Should().BeNull();

            _unitOfWorkMock.Verify(a => a.DocumentRepository.GetByIdAsync(documentId), Times.Once);
        }
        [Fact]
        public async Task DeleteDocumentAsync_Should_ReturnSuccess_WhenDocumentisDelete()
        {
            // Arrange
            var documentId = 1;
            _unitOfWorkMock.Setup(a => a.DocumentRepository.GetByIdAsync(documentId)).ReturnsAsync(new Document());

            // Act
            var result = await _documentService.DeleteDocumentAsync(documentId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Be("Document deleted successfully");
            _unitOfWorkMock.Verify(a => a.DocumentRepository.SoftRemove(It.IsAny<Document>()), Times.Once);
            _unitOfWorkMock.Verify(a => a.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateDocumentAsync_Should_ReturnSuccess_WhenDocumentExistsAndNotDeleted()
        {
            // Arrange
            var documentId = 1;
            var documentModel = new UpdateDocumentModel
            {
                DocumentLink = "DocumentLink",
                DocumentName = "Updated Document",
            };
            var document = new Document
            {
                Id = documentId,
                DocumentLink = "Old Link",
                DocumentName = "Old Document",
                IsDelete = false
            };
            _unitOfWorkMock.Setup(a => a.DocumentRepository.GetByIdAsync(documentId)).ReturnsAsync(document);

            // Act
            var result = await _documentService.UpdateDocumentAsync(documentId, documentModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            document.DocumentLink.Should().Be(documentModel.DocumentLink);
            document.DocumentName.Should().Be(documentModel.DocumentName);
        }
    }
}
