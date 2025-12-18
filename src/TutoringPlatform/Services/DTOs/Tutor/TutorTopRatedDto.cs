namespace TutoringPlatform.Services.DTOs.Tutor;

public class TutorTopRatedDto
{
    public TutorDto Tutor { get; set; } = null!;
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}