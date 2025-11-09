using System.ComponentModel.DataAnnotations;
using TaskManagement.Models.Enum;

namespace TaskManagement.Dtos;

public class TaskUpdateDto
{
    [StringLength(20)] public string? Title { get; set; } = null!;
    [StringLength(1000)] public string? Description { get; set; }
    public StatusEnum? Status { get; set; }
    public PriorityEnum? Priority { get; set; } = PriorityEnum.Medium;
    public DateTime? DueDateUtc { get; set; }
}