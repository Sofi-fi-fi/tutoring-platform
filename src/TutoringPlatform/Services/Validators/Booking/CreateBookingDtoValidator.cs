using FluentValidation;
using TutoringPlatform.Services.DTOs.Booking;

namespace TutoringPlatform.Services.Validators.Booking;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
	public CreateBookingDtoValidator()
	{
		RuleFor(field => field.StudentId)
			.GreaterThan(0).WithMessage("StudentId must be a positive integer");

		RuleFor(field => field.TutorSubjectId)
			.GreaterThan(0).WithMessage("TutorSubjectId must be a positive integer");

		RuleFor(field => field.ScheduleId)
			.GreaterThan(0).WithMessage("ScheduleId must be a positive integer");

		RuleFor(field => field.Format)
			.IsInEnum().WithMessage("Invalid booking format");
	}
}