using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Services.DTOs.TeachingLevel;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TeachingLevelController(ITeachingLevelService teachingLevelService) : ControllerBase
{
    private readonly ITeachingLevelService _teachingLevelService = teachingLevelService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TeachingLevelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TeachingLevelDto>>> GetAll()
    {
        var levels = await _teachingLevelService.GetAllAsync();
        return Ok(levels);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TeachingLevelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeachingLevelDto>> GetById(int id)
    {
        var level = await _teachingLevelService.GetByIdAsync(id);
        if (level is null)
        {
            return NotFound(new { message = $"Teaching level with ID {id} not found" });
        }

        return Ok(level);
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(TeachingLevelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeachingLevelDto>> GetByName(string name)
    {
        var level = await _teachingLevelService.GetByNameAsync(name);
        if (level is null)
        {
            return NotFound(new { message = $"Teaching level with name '{name}' not found" });
        }

        return Ok(level);
    }

    [HttpGet("position/{position:int}")]
    [ProducesResponseType(typeof(TeachingLevelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeachingLevelDto>> GetByPosition(int position)
    {
        var level = await _teachingLevelService.GetByPositionAsync(position);
        if (level is null)
        {
            return NotFound(new { message = $"Teaching level with position {position} not found" });
        }

        return Ok(level);
    }

    [HttpGet("ordered-by-position")]
    [ProducesResponseType(typeof(IEnumerable<TeachingLevelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TeachingLevelDto>>> GetAllOrdered()
    {
        var levels = await _teachingLevelService.GetOrderedByPositionAsync();
        return Ok(levels);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TeachingLevelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TeachingLevelDto>> Create([FromBody] CreateTeachingLevelDto createDto)
    {
        try
        {
            var level = await _teachingLevelService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = level.LevelId }, level);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TeachingLevelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeachingLevelDto>> Update(int id, [FromBody] UpdateTeachingLevelDto updateDto)
    {
        try
        {
            var level = await _teachingLevelService.UpdateAsync(id, updateDto);
            return Ok(level);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _teachingLevelService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}