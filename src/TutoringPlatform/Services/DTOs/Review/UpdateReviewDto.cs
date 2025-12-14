namespace TutoringPlatform.Services.DTOs.Review;

public class UpdateReviewDto
{
	public short Rating { get; set; }
	public string? Comment { get; set; }
	public bool IsAnonymous { get; set; }
}