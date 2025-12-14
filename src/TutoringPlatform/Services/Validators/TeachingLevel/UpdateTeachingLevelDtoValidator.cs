using FluentValidation;
using TutoringPlatform.Services.DTOs.TeachingLevel;

namespace TutoringPlatform.Services.Validators.TeachingLevel;

public class UpdateTeachingLevelDtoValidator : AbstractValidator<UpdateTeachingLevelDto>
{
	public UpdateTeachingLevelDtoValidator()
	{
		RuleFor(field => field.Name)
			.NotEmpty().WithMessage("Teaching level name is required")
			.MaximumLength(100).WithMessage("Teaching level name cannot exceed 100 characters")
			.Must(x => x != null && x.Any(char.IsLetter)).WithMessage("Teaching level name must contain at least one letter");
		
		RuleFor(field => field.Position)
			.GreaterThan((short)0).WithMessage("Position must be a positive number");
		
		RuleFor(field => field.Description)
			.MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
			.When(field => !string.IsNullOrEmpty(field.Description));
	}
}