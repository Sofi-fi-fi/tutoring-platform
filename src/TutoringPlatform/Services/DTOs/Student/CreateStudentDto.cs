using TutoringPlatform.Services.DTOs.User;

namespace TutoringPlatform.Services.DTOs.Student;

public class CreateStudentDto
{
	public CreateUserDto User { get; set; } = null!;
	public int? CityId { get; set; }
	public short? SchoolGrade { get; set; }
}