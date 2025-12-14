using FluentValidation;
using TutoringPlatform.Services.DTOs.Review;

namespace TutoringPlatform.Services.Validators.Review;

public class UpdateReviewDtoValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewDtoValidator()
	{
		RuleFor(field => field.Rating)
			.InclusiveBetween((short)1, (short)5).WithMessage("Rating must be between 1 and 5");

		RuleFor(field => field.Comment)
			.MaximumLength(1500).WithMessage("Comment cannot exceed 1500 characters")
			.MinimumLength(10).WithMessage("Comment must be at least 10 characters long for a valuable feedback")
			.When(field => !string.IsNullOrEmpty(field.Comment));
	}
}