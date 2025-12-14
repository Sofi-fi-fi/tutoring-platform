using FluentValidation;
using TutoringPlatform.Services.DTOs.Subject;

namespace TutoringPlatform.Services.Validators.Subject;

public class CreateSubjectDtoValidator : AbstractValidator<CreateSubjectDto>
{
	public CreateSubjectDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Subject name is required")
			.MaximumLength(100).WithMessage("Subject name cannot exceed 100 characters");

		RuleFor(x => x.Category)
			.NotEmpty().WithMessage("Subject category is required")
			.MaximumLength(100).WithMessage("Subject category cannot exceed 100 characters");

		RuleFor(x => x.Description)
			.MaximumLength(500).WithMessage("Subject description cannot exceed 500 characters")
			.When(x => !string.IsNullOrEmpty(x.Description));
	}
}