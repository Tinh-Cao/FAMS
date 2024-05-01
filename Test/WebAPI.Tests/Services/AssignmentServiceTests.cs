using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;

namespace WebAPI.Tests.Services;

public class AssignmentServiceTests : SetupTest
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentServiceTests()
    {
        _assignmentService = new AssignmentService(_unitOfWorkMock.Object, _mapperConfig);
    }

    [Fact]
    public async Task CreateAsmByExcelAsync_ShouldReturnSuccessfully()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var listName = _fixture.CreateMany<string?>(10).ToList();
        var listImport = _fixture.CreateMany<AssignmentImportModel>(10).ToList();
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetAsmsByNameAsync(It.IsAny<int>(), It.IsAny<List<string?>>()))
            .ReturnsAsync(listName);
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.AddRangeAsyncV2(It.IsAny<List<Assignment>>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
        
        // Act
        var result = await _assignmentService.CreateAsmByExcelAsync(It.IsAny<int>(), listImport);
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetAsmsByNameAsync(It.IsAny<int>(), It.IsAny<List<string?>>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.AddRangeAsyncV2(It.IsAny<List<Assignment>>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

        result.Should().BeOfType<AssignmentResponseModel>();
        Assert.Equal("Add Successfully!", result.Message);
    }
    
    [Fact]
    public async Task CreateAsmByExcelAsync_ShouldReturnFailed()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var listName = _fixture.CreateMany<string?>(10).ToList();
        var listImport = _fixture.CreateMany<AssignmentImportModel>(10).ToList();
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetAsmsByNameAsync(It.IsAny<int>(), It.IsAny<List<string?>>()))
            .ReturnsAsync(listName);
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.AddRangeAsyncV2(It.IsAny<List<Assignment>>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
        
        // Act
        var result = await _assignmentService.CreateAsmByExcelAsync(It.IsAny<int>(), listImport);
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetAsmsByNameAsync(It.IsAny<int>(), It.IsAny<List<string?>>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.AddRangeAsyncV2(It.IsAny<List<Assignment>>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

        result.Should().BeOfType<AssignmentResponseModel>();
        Assert.Equal("Add Failed!", result.Message);
    }

    [Fact]
    public async Task GetAsmsByFiltersAsync_ReturnCorrectData()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        var paginationParameter = new PaginationParameter { PageIndex = 1, PageSize = 10 };
        var assignmentFilterModel = new AssignmentFilterModel();
        
        var asmList = _fixture.Build<Assignment>().Without(x => x.Module).CreateMany(10).ToList();
        var paginatedAsm = new Pagination<Assignment>(asmList, asmList.Count, paginationParameter.PageIndex, paginationParameter.PageSize);

        // Setup
        _unitOfWorkMock
            .Setup(x => x.AssignmentRepository.GetAsmsByFiltersAsync(paginationParameter, assignmentFilterModel))
            .ReturnsAsync(paginatedAsm);
        
        // Act
        var result = await _assignmentService.GetAsmsByFiltersAsync(paginationParameter, assignmentFilterModel);
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetAsmsByFiltersAsync(paginationParameter, assignmentFilterModel), Times.Once);
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(asmList.Count);
        result.CurrentPage.Should().Be(paginationParameter.PageIndex);
        result.PageSize.Should().Be(paginationParameter.PageSize);
        result.Should().HaveCount(paginationParameter.PageSize);
    }

    [Fact]
    public async Task UpdateAsmAsync_ShouldReturnSuccessfulyMsg()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Assignment());
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.Update(It.IsAny<Assignment>()))
            .ReturnsAsync(true);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
        
        // Act
        var result = await _assignmentService.UpdateAsmAsync(It.IsAny<int>(), It.IsAny<AssignmentImportModel>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()),Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.Update(It.IsAny<Assignment>()),Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        result.Should().BeOfType<ResponseModel>();
        Assert.Equal("Assignment update successfully!", result.Message);
    }
    
    [Fact]
    public async Task UpdateAsmAsync_ShouldReturnFailedMsg()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Assignment());
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.Update(It.IsAny<Assignment>()))
            .ReturnsAsync(false);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
        
        // Act
        var result = await _assignmentService.UpdateAsmAsync(It.IsAny<int>(), It.IsAny<AssignmentImportModel>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()),Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.Update(It.IsAny<Assignment>()),Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

        result.Should().BeOfType<ResponseModel>();
        Assert.Equal("Assignment update failed!", result.Message);
    }
    
    [Fact]
    public async Task UpdateAsmAsync_ShouldReturnNotFoundMsg()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Assignment?)null);
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.Update(It.IsAny<Assignment>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _assignmentService.UpdateAsmAsync(It.IsAny<int>(), It.IsAny<AssignmentImportModel>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()),Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.Update(It.IsAny<Assignment>()),Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);

        result.Should().BeOfType<ResponseModel>();
        Assert.Equal("Assignment not found!", result.Message);
    }

    [Fact]
    public async Task GetAsmById_ShouldReturnCorrectData()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Assignment());
        
        // Act
        var result = await _assignmentService.GetAsmById(It.IsAny<int>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);
        result.Should().BeOfType<AssignmentViewModel>();
    }
    
    [Fact]
    public async Task GetAsmById_ShouldReturnNull()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Assignment?)null);
        
        // Act
        var result = await _assignmentService.GetAsmById(It.IsAny<int>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDeleteAsmById_ShouldReturnSuccessfully()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Assignment{Status = "Pending"});
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
        
        // Act
        var result = await _assignmentService.SoftDeleteAsmById(It.IsAny<int>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

        result.Should().BeOfType<ResponseModel>();
        Assert.Equal("Assignment delete successfully!", result.Message);
    }
    
    [Fact]
    public async Task SoftDeleteAsmById_ShouldReturnFailed()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Assignment{Status = "Pending"});
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
        
        // Act
        var result = await _assignmentService.SoftDeleteAsmById(It.IsAny<int>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

        result.Should().BeOfType<ResponseModel>();
        Assert.Equal("Assignment delete failed!", result.Message);
    }
    
    [Fact]
    public async Task SoftDeleteAsmById_ShouldReturnStatusMsg()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Assignment{Status = "Done"});
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
        
        // Act
        var result = await _assignmentService.SoftDeleteAsmById(It.IsAny<int>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);

        result.Should().BeOfType<ResponseModel>();
        Assert.Equal("Assignment is ongoing or is applied!", result.Message);
    }
    
    [Fact]
    public async Task SoftDeleteAsmById_ShouldReturnNotFoundMsg()
    {
        // Arrange
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Setup
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Assignment?)null);
        _unitOfWorkMock.Setup(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
        
        // Act
        var result = await _assignmentService.SoftDeleteAsmById(It.IsAny<int>());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AssignmentRepository.SoftRemove(It.IsAny<Assignment>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);

        result.Should().BeOfType<ResponseModel>();
        Assert.Equal("Assignment not found!", result.Message);
    }
}