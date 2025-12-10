using System;
using System.ComponentModel.DataAnnotations;
using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Services.DTOs.User;

public class CreateUserDto
{
	[Required(ErrorMessage = "First name is required")]
	[StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
	public string FirstName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Last name is required")]
	[StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
	public string LastName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Email is required")]
	[EmailAddress(ErrorMessage = "Invalid email address")]
	[StringLength(255, ErrorMessage = "Email cannot be longer than 255 characters")]
	public string Email { get; set; } = string.Empty;

	[Phone(ErrorMessage = "Invalid phone number")]
	[StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
	public string? Phone { get; set; } 

	[Required(ErrorMessage = "User type is required")]
	public UserType UserType { get; set; }

	public DateTime? DateOfBirth { get; set; }
}
