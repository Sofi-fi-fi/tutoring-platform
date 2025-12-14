using FluentValidation;
using TutoringPlatform.Services.DTOs.Student;

namespace TutoringPlatform.Services.Validators.Student;

public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
	public CreateStudentDtoValidator()
	{
		RuleFor(field => field.User)
			.NotNull().WithMessage("User is required")
			.SetValidator(new User.CreateUserDtoValidator());
		
		RuleFor(field => field.CityId)
			.GreaterThan(0).When(field => field.CityId.HasValue)
			.WithMessage("CityId must be greater than 0");

		RuleFor(field => field.SchoolGrade)
			.InclusiveBetween((short)1, (short)11).When(field => field.SchoolGrade.HasValue)
			.WithMessage("SchoolGrade must be between 1 and 11");
	}
}