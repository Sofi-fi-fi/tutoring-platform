using FluentValidation;
using TutoringPlatform.Services.DTOs.User;

namespace TutoringPlatform.Services.Validators.User;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
	public CreateUserDtoValidator()
	{
		RuleFor(field => field.FirstName)
			.NotEmpty().WithMessage("First name is required")
			.MaximumLength(100).WithMessage("First name cannot exceed 100 characters")
			.Matches(@"^[А-ЯІЇЄҐа-яіїєґA-Za-z'-]+$").WithMessage("First name can only contain letters, apostrophes, and hyphens");

		RuleFor(field => field.LastName)
			.NotEmpty().WithMessage("Last name is required")
			.MaximumLength(100).WithMessage("Last name cannot exceed 100 characters")
			.Matches(@"^[А-ЯІЇЄҐа-яіїєґA-Za-z'-]+$").WithMessage("Last name can only contain letters, apostrophes, and hyphens");

		RuleFor(field => field.Email)
			.NotEmpty().WithMessage("Email is required")
			.EmailAddress().WithMessage("Invalid email format")
			.MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

		RuleFor(field => field.Phone)
			.MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
			.Matches(@"^\+?[0-9]{10,19}$").WithMessage("Invalid phone number format")
			.When(field => !string.IsNullOrEmpty(field.Phone));

		RuleFor(field => field.UserType)
			.IsInEnum().WithMessage("Invalid user type");

		RuleFor(field => field.DateOfBirth)
			.LessThan(DateTime.Now).WithMessage("Date of birth must be in the past")
			.When(field => field.DateOfBirth.HasValue);
	}
}