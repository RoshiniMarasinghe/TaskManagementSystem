using System.ComponentModel.DataAnnotations;
using TaskManagement.Models.Enum;

namespace TaskManagement.Models;

public class TaskItem
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } = null!;
    [StringLength(1000)] public string? Description { get; set; }
    [Required] public StatusEnum Status { get; set; } = StatusEnum.New;
    public PriorityEnum Priority { get; set; } = PriorityEnum.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}