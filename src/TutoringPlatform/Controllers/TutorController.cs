using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Services.DTOs.Tutor;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TutorController(ITutorService tutorService) : ControllerBase
{
    private readonly ITutorService _tutorService = tutorService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TutorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorDto>>> GetAll()
    {
        var tutors = await _tutorService.GetAllAsync();
        return Ok(tutors);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TutorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorDto>> GetById(int id)
    {
        var tutor = await _tutorService.GetByIdAsync(id);
        if (tutor is null)
        {
            return NotFound(new { message = $"Tutor with ID {id} not found" });
        }

        return Ok(tutor);
    }

    [HttpGet("city/{cityId:int}")]
    [ProducesResponseType(typeof(IEnumerable<TutorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorDto>>> GetByCity(int cityId)
    {
        var tutors = await _tutorService.GetByCityAsync(cityId);
        return Ok(tutors);
    }

    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPaginated(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool descending = false)
    {
        try
        {
            var (tutors, totalCount) = await _tutorService.GetPaginatedAsync(
                pageNumber, pageSize, sortBy, descending);

            var response = new
            {
                data = tutors,
                pagination = new
                {
                    currentPage = pageNumber,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    sortBy,
                    descending
                }
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("top-rated/{count:int}")]
    [ProducesResponseType(typeof(IEnumerable<TutorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TutorDto>>> GetTopRated(int count = 10)
    {
        if (count < 1 || count > 100)
        {
            return BadRequest(new { message = "Count must be between 1 and 100" });
        }

        var tutors = await _tutorService.GetTopRatedAsync(count);
        return Ok(tutors);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TutorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TutorDto>> Create([FromBody] CreateTutorDto createDto)
    {
        try
        {
            var tutor = await _tutorService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = tutor.TutorId }, tutor);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<TutorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TutorDto>>> Search([FromBody] TutorSearchDto searchDto)
    {
        var tutors = await _tutorService.SearchTutorsAsync(searchDto);
        return Ok(tutors);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TutorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorDto>> Update(int id, [FromBody] UpdateTutorDto updateDto)
    {
        try
        {
            var tutor = await _tutorService.UpdateAsync(id, updateDto);
            return Ok(tutor);
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
            await _tutorService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}