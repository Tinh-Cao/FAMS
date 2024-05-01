using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using FluentAssertions;

namespace WebAPI.Tests.Repositories;

public class AssignmentRepositoryTests : SetupTest
{
    private readonly IAssignmentRepository _assignmentRepository;

    public AssignmentRepositoryTests()
    {
        _assignmentRepository =
            new AssignmentRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
    }

    [Fact]
    public async Task GetAsmsByFilters_ShouldReturnCorrectData()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Create mock data
        var mockData = _fixture.Build<Assignment>()
            .With(a => a.IsDelete, false)
            .Without(a => a.Module)
            .CreateMany(10).ToList();
        var data = mockData.OrderByDescending(a => a.Id).Take(5).ToList();
        var paginationParameter = new PaginationParameter{PageIndex = 1, PageSize = 5}; 
        var asmFilterModel = new AssignmentFilterModel();
        var expectedResult = new Pagination<Assignment>(data, 5, 1, 1);
        
        // Act
        await _assignmentRepository.AddRangeAsync(mockData);
        
        var save = await _dbContext.SaveChangesAsync();
        var result = await _assignmentRepository.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel);

        // Assert
        save.Should().Be(mockData.Count());
        result.Should().BeEquivalentTo(expectedResult);
        result.Should().HaveCount(paginationParameter.PageSize);
    }

    [Fact]
    public async Task AddRangeAsyncV2_ShouldReturnSuccess()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Create mock data
        var mockData = _fixture.Build<Assignment>()
            .With(a => a.IsDelete, false)
            .Without(a => a.Module)
            .CreateMany(10).ToList();
        
        // Act
        await _assignmentRepository.AddRangeAsyncV2(mockData);
        var save = await _dbContext.SaveChangesAsync();
        
        // Assert
        save.Should().Be(mockData.Count);
    }

    [Fact]
    public async Task GetAsmsByNameAsync_ShouldReturnCorrectData()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Create mock data
        var mockData = _fixture.Build<Assignment>()
            .With(a => a.IsDelete, false)
            .With(a => a.ModuleId, 1)
            .Without(a => a.Module)
            .CreateMany(10).ToList();

        var expectedResult = mockData.Select(a => a.AssignmentName).Take(9).ToList();
        var names = expectedResult.Select(a => a.ToUpper()).ToList();

        // Act
        await _assignmentRepository.AddRangeAsync(mockData);
        var save = await _dbContext.SaveChangesAsync();
        var result = await _assignmentRepository.GetAsmsByNameAsync(1, names);

        save.Should().Be(mockData.Count);
        result.Should().HaveCount(9);
        result.Should().BeEquivalentTo(expectedResult);
    }
}