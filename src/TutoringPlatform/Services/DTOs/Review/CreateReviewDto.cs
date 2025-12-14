namespace TutoringPlatform.Services.DTOs.Review;

public class CreateReviewDto
{
	public int BookingId { get; set; }
	public short Rating { get; set; }
	public string? Comment { get; set; }
	public bool IsAnonymous { get; set; }
}