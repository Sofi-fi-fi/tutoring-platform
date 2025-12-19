using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Services.DTOs.Schedule;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ScheduleController(IScheduleService scheduleService) : ControllerBase
{
    private readonly IScheduleService _scheduleService = scheduleService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAll()
    {
        var schedules = await _scheduleService.GetAllAsync();
        return Ok(schedules);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScheduleDto>> GetById(int id)
    {
        var schedule = await _scheduleService.GetByIdAsync(id);
        if (schedule is null)
        {
            return NotFound(new { message = $"Schedule with ID {id} not found" });
        }

        return Ok(schedule);
    }

    [HttpGet("tutor/{tutorId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByTutor(int tutorId)
    {
        var schedules = await _scheduleService.GetAllSlotsByTutorIdAsync(tutorId);
        return Ok(schedules);
    }

    [HttpGet("tutor/{tutorId:int}/available")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAvailableByTutor(int tutorId)
    {
        var schedules = await _scheduleService.GetAvailableSlotsByTutorAsync(tutorId);
        return Ok(schedules);
    }

    [HttpGet("tutor/{tutorId:int}/date/{date}")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByTutorAndDate(int tutorId, string date)
    {
        if (!DateOnly.TryParse(date, out var parsedDate))
        {
            return BadRequest(new { message = "Invalid date format. Use YYYY-MM-DD" });
        }

        var schedules = await _scheduleService.GetByTutorIdDateAsync(tutorId, parsedDate);
        return Ok(schedules);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ScheduleDto>> Create([FromBody] CreateScheduleDto createDto)
    {
        try
        {
            var schedule = await _scheduleService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = schedule.ScheduleId }, schedule);
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
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScheduleDto>> Update(int id, [FromBody] UpdateScheduleDto updateDto)
    {
        try
        {
            var schedule = await _scheduleService.UpdateAsync(id, updateDto);
            return Ok(schedule);
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

    [HttpPatch("{id:int}/unavailable")]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScheduleDto>> MarkAsUnavailable(int id)
    {
        try
        {
            var schedule = await _scheduleService.MarkAsUnavailableAsync(id);
            return Ok(schedule);
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

    [HttpPatch("{id:int}/available")]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScheduleDto>> MarkAsAvailable(int id)
    {
        try
        {
            var schedule = await _scheduleService.MarkAsAvailableAsync(id);
            return Ok(schedule);
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
            await _scheduleService.DeleteAsync(id);
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