using System.ComponentModel.DataAnnotations;
using TaskManagement.Models.Enum;

namespace TaskManagement.Dtos;

public class TaskCreateDto
{
    [Required] public string Title { get; set; } = null!;
    [StringLength(1000)] public string? Description { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.New;
    public PriorityEnum Priority { get; set; } = PriorityEnum.Medium;
    public DateTime? DueDateUtc { get; set; }
}