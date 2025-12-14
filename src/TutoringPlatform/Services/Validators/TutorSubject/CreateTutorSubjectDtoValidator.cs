using FluentValidation;
using TutoringPlatform.Services.DTOs.TutorSubject;

namespace TutoringPlatform.Services.Validators.TutorSubject;

public class CreateTutorSubjectDtoValidator : AbstractValidator<CreateTutorSubjectDto>
{
	public CreateTutorSubjectDtoValidator()
	{
		RuleFor(field => field.TutorId)
			.NotEmpty().WithMessage("Tutor ID is required")
			.GreaterThan(0)
			.WithMessage("Tutor ID must be a positive number");

		RuleFor(field => field.SubjectId)
			.NotEmpty().WithMessage("Subject ID is required")
			.GreaterThan(0)
			.WithMessage("Subject ID must be a positive number");
		
		RuleFor(field => field.LevelId)
			.NotEmpty().WithMessage("Level ID is required")
			.GreaterThan(0)
			.WithMessage("Level ID must be a positive number");
		
		RuleFor(field => field.HourlyRate)
			.NotEmpty().WithMessage("Hourly rate is required")
			.GreaterThan(0).WithMessage("Hourly rate must be a positive number")
			.PrecisionScale(8, 2, true).WithMessage("Hourly rate should consist of a maximum of 8 digits, of which no more than 2 can be decimal places");
	}
}