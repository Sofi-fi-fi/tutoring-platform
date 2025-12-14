using FluentValidation;
using TutoringPlatform.Services.DTOs.TutorSubject;

namespace TutoringPlatform.Services.Validators.TutorSubject;

public class UpdateTutorSubjectDtoValidator : AbstractValidator<UpdateTutorSubjectDto>
{
	public UpdateTutorSubjectDtoValidator()
	{
		RuleFor(field => field.HourlyRate)
			.NotEmpty().WithMessage("Hourly rate is required")
			.GreaterThan(0).WithMessage("Hourly rate must be a positive number")
			.PrecisionScale(8, 2, true).WithMessage("Hourly rate should consist of a maximum of 8 digits, of which no more than 2 can be decimal places");
	}
}