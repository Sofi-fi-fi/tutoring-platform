using TutoringPlatform.Services.DTOs.User;

namespace TutoringPlatform.Services.DTOs.Student;

public class StudentDto
{
	public int StudentId { get; set; }
	public int? CityId { get; set; }
	public short? SchoolGrade { get; set; }
	public UserDto User { get; set; } = null!;
}