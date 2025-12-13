using FluentValidation;
using TutoringPlatform.Services.DTOs.Tutor;

namespace TutoringPlatform.Services.Validators.Tutor;

public class CreateTutordtovalidator : AbstractValidator<CreateTutorDto>
{
	public CreateTutordtovalidator()
	{
		RuleFor(field => field.CityId)
			.GreaterThan(0).When(x => x.CityId.HasValue)
			.WithMessage("City ID must be a positive number");

		RuleFor(field => field.YearsExperience)
			.NotEmpty().WithMessage("Years of experience is required")
			.GreaterThan((short)0)
			.WithMessage("Years of experience must be a positive number");

		RuleFor(field => field.Education)
			.NotEmpty().WithMessage("Education is required")
			.MaximumLength(2000).WithMessage("Education cannot exceed 2000 characters");

		RuleFor(field => field)
			.Must(field => field.OnlineAvailable || field.OfflineAvailable)
			.WithMessage("At least one of online or offline availability must be true");

		RuleFor(field => field.AboutMe)
			.MaximumLength(2000)
			.WithMessage("About Me section cannot exceed 2000 characters").When(field => !string.IsNullOrEmpty(field.AboutMe));

		RuleFor(field => field.Address)
			.NotEmpty().When(field => field.OfflineAvailable)
			.WithMessage("Address is required when offline availability is true");
	}
}	