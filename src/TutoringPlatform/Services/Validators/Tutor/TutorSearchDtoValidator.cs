using FluentValidation;
using TutoringPlatform.Services.DTOs.Tutor;

namespace TutoringPlatform.Services.Validators.Tutor;

public class TutorSearchDtoValidator : AbstractValidator<TutorSearchDto>
{
	public TutorSearchDtoValidator()
	{
		RuleFor(field => field.CityID)
			.GreaterThan(0).When(field => field.CityID.HasValue)
			.WithMessage("City ID must be a positive number");

		RuleFor(field => field.SubjectId)
			.GreaterThan(0).When(field => field.SubjectId.HasValue)
			.WithMessage("Subject ID must be a positive number");

		RuleFor(field => field.LevelId)
			.GreaterThan(0).When(field => field.LevelId.HasValue)
			.WithMessage("Level ID must be a positive number");

		RuleFor(field => field.MinPrice)
			.GreaterThanOrEqualTo(0).When(field => field.MinPrice.HasValue)
			.WithMessage("Minimum price must be a non-negative number");

		RuleFor(field => field.MaxPrice)
			.GreaterThanOrEqualTo(0).When(field => field.MaxPrice.HasValue)
			.WithMessage("Maximum price must be a non-negative number");

		RuleFor(field => field)
			.Must(field => !field.MinPrice.HasValue || !field.MaxPrice.HasValue || field.MinPrice <= field.MaxPrice)
			.WithMessage("Minimum price cannot be greater than maximum price");

		RuleFor(field => field)
			.Must(field => field.OnlineOnly is true || field.OfflineOnly is true)
			.WithMessage("At least one of online or offline availability must be true");
	}
}