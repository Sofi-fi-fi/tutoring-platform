using Microsoft.AspNetCore.Mvc;
using TutoringPlatform.Services.DTOs.Review;
using TutoringPlatform.Services.Interfaces;

namespace TutoringPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReviewController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
    {
        var reviews = await _reviewService.GetAllAsync();
        return Ok(reviews);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewDto>> GetById(int id)
    {
        var review = await _reviewService.GetByIdAsync(id);
        if (review is null)
        {
            return NotFound(new { message = $"Review with ID {id} not found" });
        }

        return Ok(review);
    }

    [HttpGet("booking/{bookingId:int}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewDto>> GetByBooking(int bookingId)
    {
        var review = await _reviewService.GetByBookingAsync(bookingId);
        if (review is null)
        {
            return NotFound(new { message = $"Review for Booking ID {bookingId} not found" });
        }

        return Ok(review);
    }

    [HttpGet("student/{studentId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByStudent(int studentId)
    {
        var reviews = await _reviewService.GetByStudentAsync(studentId);
        return Ok(reviews);
    }

    [HttpGet("tutor/{tutorId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByTutor(int tutorId)
    {
        var reviews = await _reviewService.GetByTutorAsync(tutorId);
        return Ok(reviews);
    }

    [HttpGet("rating/{rating:int}")]
    [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByRating(short rating)
    {
        if (rating < 1 || rating > 5)
        {
            return BadRequest(new { message = "Rating must be between 1 and 5" });
        }

        var reviews = await _reviewService.GetByRatingAsync(rating);
        return Ok(reviews);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto createDto)
    {
        try
        {
            var review = await _reviewService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewDto>> Update(int id, [FromBody] UpdateReviewDto updateDto)
    {
        try
        {
            var review = await _reviewService.UpdateAsync(id, updateDto);
            return Ok(review);
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
            await _reviewService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}