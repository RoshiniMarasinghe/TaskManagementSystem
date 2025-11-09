using TaskManagement.Models;

namespace TaskManagement.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task AddTaskAsync(TaskItem task);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(TaskItem task);
}