namespace TutoringPlatform.Services.DTOs.TeachingLevel;

public class UpdateTeachingLevelDto
{
	public string Name { get; set; } = string.Empty;
	public short Position { get; set; }
	public string? Description { get; set; }

}