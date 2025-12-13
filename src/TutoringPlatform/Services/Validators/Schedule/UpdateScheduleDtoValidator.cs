using FluentValidation;
using TutoringPlatform.Services.DTOs.Schedule;

namespace TutoringPlatform.Services.Validators.Schedule;

public class UpdateScheduleDtoValidator : AbstractValidator<UpdateScheduleDto>
{
	public UpdateScheduleDtoValidator()
	{
		RuleFor(field => field.Date)
			.GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
			.WithMessage("Date cannot be in the past");

		RuleFor(field => field.StartTime)
			.NotEmpty().WithMessage("StartTime is required");

		RuleFor(field => field.EndTime)
			.NotEmpty().WithMessage("EndTime is required");
		
		RuleFor(field => field)
			.Must(dto => dto.EndTime > dto.StartTime)
			.WithMessage("EndTime must be later than StartTime");

		RuleFor(field => field)
			.Must(dto => (dto.EndTime - dto.StartTime).TotalMinutes == 60)
			.WithMessage("The duration between StartTime and EndTime must be exactly 60 minutes");
	}
}