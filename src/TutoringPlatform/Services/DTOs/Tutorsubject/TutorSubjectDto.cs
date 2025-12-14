namespace TutoringPlatform.Services.DTOs.TutorSubject;

public class TutorSubjectDto
{
	public int TutorSubjectId { get; set; }
	public int TutorId { get; set; }
	public int SubjectId { get; set; }
	public int LevelId { get; set; }
	public decimal HourlyRate { get; set; }
}