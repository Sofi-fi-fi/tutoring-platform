using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Services.DTOs.User;

public class CreateUserDto
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Phone { get; set; } 
	public UserType UserType { get; set; }
	public DateTime? DateOfBirth { get; set; }
}