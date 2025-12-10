using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Services.DTOs.User;

public class UserDto
{
	public int UserId { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Phone { get; set; }	
	public UserType UserType { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public DateTime RegistrationDate { get; set; } 
}