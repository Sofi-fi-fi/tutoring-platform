using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.Review;
using TutoringPlatform.Services.Validators.Review;

namespace TutoringPlatform.Tests.Services.Validators.Review;

public class CreateReviewDtoValidatorTests
{
	private readonly CreateReviewDtoValidator _validator;

	public CreateReviewDtoValidatorTests()
	{
		_validator = new CreateReviewDtoValidator();
	}

	#region BookingId Tests

	[Fact]
	public void BookingId_WhenZero_ResultError()
	{
		var model = new CreateReviewDto { BookingId = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.BookingId)
			.WithErrorMessage("BookingId must be greater than 0");
	}

	[Fact]
	public void BookingId_WhenNegative_ResultError()
	{
		var model = new CreateReviewDto { BookingId = -1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.BookingId)
			.WithErrorMessage("BookingId must be greater than 0");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(999999)]
	public void BookingId_WhenPositive_ResultOk(int bookingId)
	{
		var model = new CreateReviewDto
		{
			BookingId = bookingId,
			Rating = 5,
			Comment = "Great tutoring session!"
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.BookingId);
	}

	#endregion

	#region Rating Tests

	[Theory]
	[InlineData((short)0)]
	[InlineData((short)-1)]
	[InlineData((short)-5)]
	public void Rating_WhenBelowMinimum_ResultError(short rating)
	{
		var model = new CreateReviewDto { Rating = rating };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Rating)
			.WithErrorMessage("Rating must be between 1 and 5");
	}

	[Theory]
	[InlineData((short)6)]
	[InlineData((short)10)]
	[InlineData((short)100)]
	public void Rating_WhenAboveMaximum_ResultError(short rating)
	{
		var model = new CreateReviewDto { Rating = rating };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Rating)
			.WithErrorMessage("Rating must be between 1 and 5");
	}

	[Theory]
	[InlineData((short)1)]
	[InlineData((short)2)]
	[InlineData((short)3)]
	[InlineData((short)4)]
	[InlineData((short)5)]
	public void Rating_WhenValid_ResultOk(short rating)
	{
		var model = new CreateReviewDto
		{
			BookingId = 1,
			Rating = rating,
			Comment = "Umm, dunno what to write here"
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Rating);
	}

	#endregion

	#region Comment Tests

	[Fact]
	public void Comment_WhenNull_ResultOk()
	{
		var model = new CreateReviewDto
		{
			BookingId = 1,
			Rating = 5,
			Comment = null
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Comment);
	}

	[Fact]
	public void Comment_WhenEmpty_ResultOk()
	{
		var model = new CreateReviewDto
		{
			BookingId = 1,
			Rating = 5,
			Comment = string.Empty
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Comment);
	}

	[Theory]
	[InlineData("Short")]
	[InlineData("Too bad")]
	[InlineData("123456789")]
	public void Comment_WhenBelowMinimumLength_ResultError(string comment)
	{
		var model = new CreateReviewDto
		{
			BookingId = 1,
			Rating = 5,
			Comment = comment
		};

		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Comment)
			.WithErrorMessage("Comment must be at least 10 characters long for a valuable feedback");
	}

	[Fact]
	public void Comment_WhenExceedsMaximumLength_ResultError()
	{
		var model = new CreateReviewDto
		{
			BookingId = 1,
			Rating = 5,
			Comment = new string('a', 1501)
		};

		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Comment)
			.WithErrorMessage("Comment cannot exceed 1500 characters");
	}

	[Theory]
	[InlineData("This is a decent review!")]
	[InlineData("Great tutoring session!")]
	[InlineData("I learned a lot from this session and I love KPI University!!!")]
	public void Comment_WhenValid_ResultOk(string comment)
	{
		var model = new CreateReviewDto
		{
			BookingId = 1,
			Rating = 5,
			Comment = comment
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Comment);
	}

	[Fact]
	public void Comment_WhenMaximumLength_ResultOk()
	{
		var model = new CreateReviewDto
		{
			BookingId = 1,
			Rating = 5,
			Comment = new string('a', 1500)
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Comment);
	}

	#endregion
}
