using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.TutorSubject;
using TutoringPlatform.Services.Validators.TutorSubject;

namespace TutoringPlatform.Tests.Services.Validators.TutorSubject;

public class UpdateTutorSubjectDtoValidatorTests
{
	private readonly UpdateTutorSubjectDtoValidator _validator;

	public UpdateTutorSubjectDtoValidatorTests()
	{
		_validator = new UpdateTutorSubjectDtoValidator();
	}

	#region HourlyRate Tests

	[Fact]
	public void HourlyRate_WhenZero_ResultError()
	{
		var model = new UpdateTutorSubjectDto { HourlyRate = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.HourlyRate)
			.WithErrorMessage("Hourly rate is required");
	}

	[Fact]
	public void HourlyRate_WhenNegative_ResultError()
	{
		var model = new UpdateTutorSubjectDto { HourlyRate = -10.50m };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.HourlyRate)
			.WithErrorMessage("Hourly rate must be a positive number");
	}

	[Fact]
	public void HourlyRate_WhenExceedsPrecision_ResultError()
	{
		var model = new UpdateTutorSubjectDto { HourlyRate = 1234567.89m };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.HourlyRate)
			.WithErrorMessage("Hourly rate should consist of a maximum of 8 digits, of which no more than 2 can be decimal places");
	}

	[Fact]
	public void HourlyRate_WhenExceedsScale_ResultError()
	{
		var model = new UpdateTutorSubjectDto { HourlyRate = 100.999m };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.HourlyRate)
			.WithErrorMessage("Hourly rate should consist of a maximum of 8 digits, of which no more than 2 can be decimal places");
	}

	[Theory]
	[InlineData(0.01)]
	[InlineData(1.00)]
	[InlineData(50.50)]
	[InlineData(100.99)]
	[InlineData(500.00)]
	[InlineData(999999.99)]
	public void HourlyRate_WhenValid_ResultOk(decimal hourlyRate)
	{
		var model = new UpdateTutorSubjectDto
		{
			HourlyRate = hourlyRate
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.HourlyRate);
	}

	#endregion
}