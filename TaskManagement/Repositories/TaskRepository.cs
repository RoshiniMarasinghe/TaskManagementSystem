using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Repositories;

public class TaskRepository(AppDbContext context) : ITaskRepository
{
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await context.TaskItems.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await context.TaskItems.FindAsync(id);
    }

    public async Task AddTaskAsync(TaskItem task)
    {
        await context.AddAsync(task);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        context.Update(task);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(TaskItem task)
    {
        context.Remove(task);
        await context.SaveChangesAsync();
    }
}