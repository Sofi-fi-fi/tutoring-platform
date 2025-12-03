using System;
using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Models.Entities;

public class User
{
	public int UserId { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string PasswordHash { get; set; } = null!;
	public string? Phone { get; set; } 
	public UserType UserType { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public DateTime RegistrationDate { get; set; }

	public virtual Student? Student { get; set; }
	public virtual Tutor? Tutor { get; set; }
}
