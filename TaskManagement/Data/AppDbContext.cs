using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var task = modelBuilder.Entity<TaskItem>();

        task.Property(t => t.Status).HasConversion<string>();
        task.Property(t => t.Priority).HasConversion<string>();

        task.HasIndex(t => t.Status);
    }
}