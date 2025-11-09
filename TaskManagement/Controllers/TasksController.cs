using Microsoft.AspNetCore.Mvc;
using TaskManagement.Dtos;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await taskService.GetAllAsync();
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await taskService.GetByIdAsync(id);
        return result.Success ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskCreateDto dto)
    {
        var result = await taskService.CreateAsync(dto);
        if (!result.Success) return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
    {
        var result = await taskService.UpdateAsync(id, dto);
        if (!result.Success)
        {
            if (result.Error != null && result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await taskService.DeleteAsync(id);
        if (!result.Success)
        {
            if (result.Error != null && result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return NoContent();
    }
}