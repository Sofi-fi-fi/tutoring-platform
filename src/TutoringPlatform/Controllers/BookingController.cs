using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Services.DTOs.Booking;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(bookings);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingDto>> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        
        if (booking is null)
        {
            return NotFound(new { message = $"Booking with ID {id} not found" });
        }

        return Ok(booking);
    }

    [HttpGet("student/{studentId:int}")]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetByStudent(int studentId)
    {
        var bookings = await _bookingService.GetByStudentAsync(studentId);
        return Ok(bookings);
    }

    [HttpGet("tutor/{tutorId:int}")]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetByTutor(int tutorId)
    {
        var bookings = await _bookingService.GetByTutorAsync(tutorId);
        return Ok(bookings);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetByStatus(string status)
    {
        if (!Enum.TryParse<BookingStatus>(status, true, out var bookingStatus))
        {
            return BadRequest(new { message = $"Invalid booking status: {status}" });
        }

        var bookings = await _bookingService.GetByStatusAsync(bookingStatus);
        return Ok(bookings);
    }

    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPaginated(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] BookingStatus? status = null,
        [FromQuery] int? studentId = null,
        [FromQuery] int? tutorId = null)
    {
        try
        {
            var (bookings, totalCount) = await _bookingService.GetPaginatedAsync(
                pageNumber, pageSize, status, studentId, tutorId);

            var response = new
            {
                data = bookings,
                pagination = new
                {
                    currentPage = pageNumber,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto createDto)
    {
        try
        {
            var booking = await _bookingService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingDto>> Update(int id, [FromBody] UpdateBookingDto updateDto)
    {
        try
        {
            var booking = await _bookingService.UpdateAsync(id, updateDto);
            return Ok(booking);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _bookingService.DeleteAsync(id);
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