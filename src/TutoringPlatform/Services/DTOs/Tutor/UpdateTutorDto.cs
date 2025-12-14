using TutoringPlatform.Services.DTOs.User;

namespace TutoringPlatform.Services.DTOs.Tutor;

public class UpdateTutorDto
{
	public UpdateUserDto User { get; set; } = null!;
	public int? CityId { get; set; }
	public short YearsExperience { get; set; }
	public string Education { get; set; } = string.Empty;
	public bool OnlineAvailable { get; set; }
	public bool OfflineAvailable { get; set; }
	public string? AboutMe { get; set; }
	public string? Address { get; set; }
}