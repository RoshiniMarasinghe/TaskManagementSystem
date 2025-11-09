using Moq;
using TaskManagement.Dtos;
using TaskManagement.Models;
using TaskManagement.Models.Enum;
using TaskManagement.Repositories;
using TaskManagement.Services;

namespace TaskManagement.Tests.UnitTests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _service = new TaskService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenExistingTasks_WhenCallingGetAllAsync_ShouldReturnAllTasks()
    {
        // Given
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Task 1" },
            new TaskItem { Id = 2, Title = "Task 2" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(tasks);

        // When
        var result = await _service.GetAllAsync();

        // Should
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, ((List<TaskItem>)result.Data!).Count);
    }

    [Fact]
    public async Task GivenExistingTask_WhenCallingGetByIdAsync_ShouldReturnTask()
    {
        // Given
        var task = new TaskItem { Id = 10, Title = "Existing" };

        _repositoryMock.Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(task);

        // When
        var result = await _service.GetByIdAsync(10);

        // Should
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(10, result.Data!.Id);
        Assert.Equal("Existing", result.Data.Title);
    }

    [Fact]
    public async Task GivenNonExistingTask_WhenCallingGetByIdAsync_ShouldReturnFail()
    {
        // Given
        _repositoryMock.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((TaskItem?)null);

        // When
        var result = await _service.GetByIdAsync(999);

        // Should
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
        Assert.Contains("not found", result.Error!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GivenPastDueDate_WhenCallingCreateAsync_ShouldReturnFail()
    {
        // Given
        var dto = new TaskCreateDto
        {
            Title = "Test",
            Description = "Something",
            Status = StatusEnum.New,
            Priority = PriorityEnum.Medium,
            DueDateUtc = DateTime.UtcNow.AddDays(-1)
        };

        // When
        var result = await _service.CreateAsync(dto);

        // Should
        Assert.False(result.Success);
        Assert.Equal("Due date cannot be in the past.", result.Error);
        _repositoryMock.Verify(r => r.AddTaskAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task GivenValidTaskCreateDto_WhenCallingCreateAsync_ShouldReturnSuccess()
    {
        // Given
        var dto = new TaskCreateDto
        {
            Title = "Valid Task",
            Description = "desc",
            Status = StatusEnum.New,
            Priority = PriorityEnum.Medium,
            DueDateUtc = DateTime.UtcNow.AddDays(1)
        };

        // When
        var result = await _service.CreateAsync(dto);

        // Should
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Valid Task", result.Data!.Title);
        _repositoryMock.Verify(r => r.AddTaskAsync(It.IsAny<TaskItem>()), Times.Once);
    }


    [Fact]
    public async Task GivenNonExistingTask_WhenCallingUpdateAsync_ShouldReturnFail()
    {
        // Given
        _repositoryMock.Setup(r => r.GetByIdAsync(5))
            .ReturnsAsync((TaskItem?)null);

        var dto = new TaskUpdateDto
        {
            Title = "New Test Task 01"
        };

        // When
        var result = await _service.UpdateAsync(5, dto);

        // Should
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
        Assert.Contains("not found", result.Error!, StringComparison.OrdinalIgnoreCase);

        _repositoryMock.Verify(r => r.UpdateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task GivenPastDueDate_WhenCallingUpdateAsync_ShouldReturnFail()
    {
        // Given
        var existing = new TaskItem
        {
            Id = 1,
            Title = "Original title",
            Status = StatusEnum.New,
            Priority = PriorityEnum.Medium
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existing);

        var dto = new TaskUpdateDto
        {
            DueDateUtc = DateTime.UtcNow.AddDays(-1)
        };

        // When
        var result = await _service.UpdateAsync(1, dto);

        // Should
        Assert.False(result.Success);
        Assert.Equal("Due date cannot be in the past.", result.Error);
        _repositoryMock.Verify(r => r.UpdateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task GivenPartialUpdate_WhenCallingUpdateAsync_ShouldUpdateOnlyProvidedFields()
    {
        // Given
        var existing = new TaskItem
        {
            Id = 2,
            Title = "Original title",
            Description = "Original desc",
            Status = StatusEnum.New,
            Priority = PriorityEnum.Medium,
            DueDate = DateTime.UtcNow.AddDays(2)
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(2))
            .ReturnsAsync(existing);

        var dto = new TaskUpdateDto
        {
            Title = "Updated title",
            Priority = PriorityEnum.High
        };

        // When
        var result = await _service.UpdateAsync(2, dto);

        // Should
        Assert.True(result.Success);
        Assert.True(result.Data);

        // unchanged fields remain the same
        Assert.Equal(StatusEnum.New, existing.Status);
        Assert.NotEqual("Original title", existing.Title);

        _repositoryMock.Verify(r => r.UpdateTaskAsync(It.Is<TaskItem>(t =>
            t.Id == 2 &&
            t.Title == "Updated title" &&
            t.Description == "Original desc" &&
            t.Status == StatusEnum.New &&
            t.Priority == PriorityEnum.High
        )), Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingTask_WhenCallingDeleteAsync_ShouldReturnFail()
    {
        // Given
        _repositoryMock.Setup(r => r.GetByIdAsync(100))
            .ReturnsAsync((TaskItem?)null);

        // When
        var result = await _service.DeleteAsync(1);

        // Should
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
        Assert.Contains("not found", result.Error!);

        _repositoryMock.Verify(r => r.DeleteTaskAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task GivenExistingTask_WhenCallingDeleteAsync_ShouldDeleteAndReturnSuccess()
    {
        // Given
        var existing = new TaskItem { Id = 3 };

        _repositoryMock.Setup(r => r.GetByIdAsync(3))
            .ReturnsAsync(existing);

        // When
        var result = await _service.DeleteAsync(3);

        // Should
        Assert.True(result.Success);
        Assert.True(result.Data);
        _repositoryMock.Verify(r => r.DeleteTaskAsync(existing), Times.Once);
    }
}