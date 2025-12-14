using TutoringPlatform.Services.DTOs.User;

namespace TutoringPlatform.Services.DTOs.Tutor;

public class TutorDto
{
	public int TutorId { get; set; }
	public int? CityId { get; set; }
	public short YearsExperience { get; set; }
	public string Education { get; set; } = string.Empty;
	public string? AboutMe { get; set; }
	public bool OnlineAvailable { get; set; }
	public bool OfflineAvailable { get; set; }
	public string? Address { get; set; }
	public UserDto User { get; set; } = null!;
}