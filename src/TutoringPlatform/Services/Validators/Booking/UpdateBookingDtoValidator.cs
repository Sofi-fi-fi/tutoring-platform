using FluentValidation;
using TutoringPlatform.Services.DTOs.Booking;

namespace TutoringPlatform.Services.Validators.Booking;

public class UpdateBookingDtoValidator : AbstractValidator<UpdateBookingDto>
{
	public UpdateBookingDtoValidator()
	{
		RuleFor(field => field.Status)
			.IsInEnum().WithMessage("Invalid booking status");
	}
}