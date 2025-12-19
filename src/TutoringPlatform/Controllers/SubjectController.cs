using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Services.DTOs.Subject;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SubjectController(ISubjectService subjectService) : ControllerBase
{
    private readonly ISubjectService _subjectService = subjectService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll()
    {
        var subjects = await _subjectService.GetAllAsync();
        return Ok(subjects);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubjectDto>> GetById(int id)
    {
        var subject = await _subjectService.GetByIdAsync(id);
        if (subject is null)
        {
            return NotFound(new { message = $"Subject with ID {id} not found" });
        }

        return Ok(subject);
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubjectDto>> GetByName(string name)
    {
        var subject = await _subjectService.GetByNameAsync(name);
        if (subject is null)
        {
            return NotFound(new { message = $"Subject with name '{name}' not found" });
        }

        return Ok(subject);
    }

    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(IEnumerable<SubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetByCategory(string category)
    {
        var subjects = await _subjectService.GetByCategoryAsync(category);
        return Ok(subjects);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubjectDto>> Create([FromBody] CreateSubjectDto createDto)
    {
        try
        {
            var subject = await _subjectService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = subject.SubjectId }, subject);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubjectDto>> Update(int id, [FromBody] UpdateSubjectDto updateDto)
    {
        try
        {
            var subject = await _subjectService.UpdateAsync(id, updateDto);
            return Ok(subject);
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
            await _subjectService.DeleteAsync(id);
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