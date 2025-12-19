using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Services.DTOs.TutorSubject;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TutorSubjectController(ITutorSubjectService tutorSubjectService) : ControllerBase
{
    private readonly ITutorSubjectService _tutorSubjectService = tutorSubjectService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TutorSubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorSubjectDto>>> GetAll()
    {
        var tutorSubjects = await _tutorSubjectService.GetAllAsync();
        return Ok(tutorSubjects);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TutorSubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorSubjectDto>> GetById(int id)
    {
        var tutorSubject = await _tutorSubjectService.GetByIdAsync(id);
        if (tutorSubject is null)
        {
            return NotFound(new { message = $"Tutor-subject with ID {id} not found" });
        }

        return Ok(tutorSubject);
    }

    [HttpGet("tutor/{tutorId:int}")]
    [ProducesResponseType(typeof(IEnumerable<TutorSubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorSubjectDto>>> GetByTutor(int tutorId)
    {
        var tutorSubjects = await _tutorSubjectService.GetByTutorIdAsync(tutorId);
        return Ok(tutorSubjects);
    }

    [HttpGet("subject/{subjectId:int}")]
    [ProducesResponseType(typeof(IEnumerable<TutorSubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorSubjectDto>>> GetBySubject(int subjectId)
    {
        var tutorSubjects = await _tutorSubjectService.GetBySubjectIdAsync(subjectId);
        return Ok(tutorSubjects);
    }

    [HttpGet("level/{levelId:int}")]
    [ProducesResponseType(typeof(IEnumerable<TutorSubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorSubjectDto>>> GetByLevel(int levelId)
    {
        var tutorSubjects = await _tutorSubjectService.GetByLevelIdAsync(levelId);
        return Ok(tutorSubjects);
    }

    [HttpGet("pricing-statistics")]
    [ProducesResponseType(typeof(IEnumerable<TutorPricingStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetTutorPricingStatistics()
    {
        var statistics = await _tutorSubjectService.GetTutorPricingStatisticsAsync();
        return Ok(statistics);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TutorSubjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TutorSubjectDto>> Create([FromBody] CreateTutorSubjectDto createDto)
    {
        try
        {
            var tutorSubject = await _tutorSubjectService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = tutorSubject.TutorSubjectId }, tutorSubject);
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

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TutorSubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorSubjectDto>> Update(int id, [FromBody] UpdateTutorSubjectDto updateDto)
    {
        try
        {
            var tutorSubject = await _tutorSubjectService.UpdateAsync(id, updateDto);
            return Ok(tutorSubject);
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
            await _tutorSubjectService.DeleteAsync(id);
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
