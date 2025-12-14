using FluentValidation;
using TutoringPlatform.Services.DTOs.City;

namespace TutoringPlatform.Services.Validators.City;

public class CreateCityDtoValidator : AbstractValidator<CreateCityDto>
{
	public CreateCityDtoValidator()
	{
		RuleFor(field => field.Name)
			.NotEmpty().WithMessage("City name is required")
			.MaximumLength(100).WithMessage("City name must not exceed 100 characters")
			.Matches(@"^[А-ЯІЇЄҐа-яіїєґA-Za-z\s'-]+$").WithMessage("City name contains invalid characters");

		RuleFor(field => field.Region)
			.MaximumLength(100).WithMessage("Region must not exceed 100 characters")
			.Matches(@"^[А-ЯІЇЄҐа-яіїєґA-Za-z\s'-]+$").WithMessage("Region contains invalid characters")
			.When(field => !string.IsNullOrEmpty(field.Region));

		RuleFor(field => field.Country)
			.NotEmpty().WithMessage("Country is required")
			.MaximumLength(100).WithMessage("Country must not exceed 100 characters")
			.Matches(@"^[А-ЯІЇЄҐа-яіїєґA-Za-z\s'-]+$").WithMessage("Country contains invalid characters");
	}
}