using TaskManagement.Dtos;
using TaskManagement.Helper;
using TaskManagement.Models;
using TaskManagement.Models.Enum;
using TaskManagement.Repositories;

namespace TaskManagement.Services;

public class TaskService(ITaskRepository repository) : ITaskService
{
    public async Task<ServiceResult<IEnumerable<TaskItem>>> GetAllAsync()
    {
        var tasks = await repository.GetAllAsync();
        return ServiceResult<IEnumerable<TaskItem>>.Ok(tasks);
    }

    public async Task<ServiceResult<TaskItem>> GetByIdAsync(int id)
    {
        var task = await repository.GetByIdAsync(id);
        if (task is null)
            return ServiceResult<TaskItem>.Fail($"Task with ID {id} not found.");

        return ServiceResult<TaskItem>.Ok(task);
    }

    public async Task<ServiceResult<TaskItem>> CreateAsync(TaskCreateDto dto)
    {
        if (dto.DueDateUtc is not null && dto.DueDateUtc.Value.Date < DateTime.UtcNow.Date)
            return ServiceResult<TaskItem>.Fail("Due date cannot be in the past.");

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDateUtc,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await repository.AddTaskAsync(task);
        return ServiceResult<TaskItem>.Ok(task);
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, TaskUpdateDto dto)
    {
        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return ServiceResult<bool>.Fail($"Task with ID {id} not found.");

        if (dto.DueDateUtc is not null && dto.DueDateUtc.Value.Date < DateTime.UtcNow.Date)
            return ServiceResult<bool>.Fail("Due date cannot be in the past.");

        if (!string.IsNullOrWhiteSpace(dto.Title))
            existing.Title = dto.Title;

        if (dto.Description is not null)
            existing.Description = dto.Description;

        if (dto.Status.HasValue)
            existing.Status = dto.Status.Value;

        if (dto.Priority.HasValue)
            existing.Priority = dto.Priority.Value;

        if (dto.DueDateUtc.HasValue)
            existing.DueDate = dto.DueDateUtc.Value;

        existing.UpdatedDate = DateTime.UtcNow;

        await repository.UpdateTaskAsync(existing);
        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return ServiceResult<bool>.Fail($"Task with ID {id} not found.");

        await repository.DeleteTaskAsync(existing);
        return ServiceResult<bool>.Ok(true);
    }
}