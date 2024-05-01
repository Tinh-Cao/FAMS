using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Repositories
{
    public class DocumentRepositoryTests : SetupTest
    {
        private readonly IDocumentRepository _documentRepository;
        public DocumentRepositoryTests()
        {
            _documentRepository = new DocumentRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }
        [Fact]
        public async Task AddCreateAsync_Should_ReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Create mock Document
            var mockDocument = _fixture.Build<Document>()
                .Without(l => l.Lesson)
                .CreateMany(10).ToList();
            await _dbContext.Documents.AddRangeAsync(mockDocument);
            // Act
            var saveChanges = await _dbContext.SaveChangesAsync();
            var Result = await _documentRepository.GetAllAsync();
            // Assert
            saveChanges.Should().Be(mockDocument.Count());
            Result.Should().BeEquivalentTo(mockDocument);
        }
        [Fact]
        public async Task GetDocumentDetails_ExistingModule_ShouldReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Create mock Document
            var mockDocument = _fixture.Build<Document>()
                .Without(l => l.Lesson)
                .Create();
            // Act
            var addedDocument = await _documentRepository.AddAsync(mockDocument);
            await _dbContext.SaveChangesAsync();
            var result = await _documentRepository.GetByIdAsync(addedDocument.Id);
            // Assert
            result.Should().BeEquivalentTo(addedDocument);
        }
        [Fact]
        public async Task UpdateDocument_Should_ReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            //Create mock Document
            var mockDocument = _fixture.Build<Document>()
                .Without(l => l.Lesson)
                .Create();
            await _documentRepository.AddAsync(mockDocument);
            await _dbContext.SaveChangesAsync();
            // Act
            mockDocument.DocumentName = "New Document";
            await _documentRepository.Update(mockDocument);
            await _dbContext.SaveChangesAsync();
            var result = await _documentRepository.GetByIdAsync(mockDocument.Id);
            // Assert
            result.Should().BeEquivalentTo(mockDocument);
            result.DocumentName.Should().Be(mockDocument.DocumentName);
        }
        [Fact]
        public async Task DeleteDocumentAsync_Should_ReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            //Create mock Document
            var mockDocument = _fixture.Build<Document>()
                .With(m => m.IsDelete, false)
                .Without(l => l.Lesson)
                .Create();
            await _dbContext.Documents.AddAsync(mockDocument);
            await _dbContext.SaveChangesAsync();
            // Act
            await _documentRepository.SoftRemove(mockDocument);
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _documentRepository.GetByIdAsync(mockDocument.Id);
            // Assert
            saveChanges.Should().Be(1);
            result.Should().NotBeNull();
            result?.IsDelete.Should().BeTrue();
        }
    }
}
