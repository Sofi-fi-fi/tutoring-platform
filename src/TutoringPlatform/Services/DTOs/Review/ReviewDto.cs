namespace TutoringPlatform.Services.DTOs.Review;

public class ReviewDto
{
	public int ReviewId { get; set; }
	public int BookingId { get; set; }
	public short Rating { get; set; }
	public string? Comment { get; set; }
	public DateTime CreatedAt { get; set; }
	public bool IsAnonymous { get; set; }
}