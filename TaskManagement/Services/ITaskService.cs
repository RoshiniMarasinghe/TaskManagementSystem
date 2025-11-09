using TaskManagement.Dtos;
using TaskManagement.Helper;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Services;

public interface ITaskService
{
    Task<ServiceResult<IEnumerable<TaskItem>>> GetAllAsync();
    Task<ServiceResult<TaskItem>> GetByIdAsync(int id);
    Task<ServiceResult<TaskItem>> CreateAsync(TaskCreateDto dto);
    Task<ServiceResult<bool>> UpdateAsync(int id, TaskUpdateDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}