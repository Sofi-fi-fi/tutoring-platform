namespace TutoringPlatform.Services.DTOs.TeachingLevel;

public class TeachingLevelDto
{
	public int LevelId { get; set; }
	public string Name { get; set; } = string.Empty;
	public short Position { get; set; }
	public string? Description { get; set; }
}