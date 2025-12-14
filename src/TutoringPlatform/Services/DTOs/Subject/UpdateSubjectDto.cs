namespace TutoringPlatform.Services.DTOs.Subject;

public class UpdateSubjectDto
{
	public string Name { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public string? Description { get; set; }
}