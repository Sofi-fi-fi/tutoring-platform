using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.TutorSubject;
using TutoringPlatform.Services.Validators.TutorSubject;

namespace TutoringPlatform.Tests.Services.Validators.TutorSubject;

public class CreateTutorSubjectDtoValidatorTests
{
	private readonly CreateTutorSubjectDtoValidator _validator;

	public CreateTutorSubjectDtoValidatorTests()
	{
		_validator = new CreateTutorSubjectDtoValidator();
	}

	#region TutorId Tests

	[Fact]
	public void TutorId_WhenEmpty_ResultError()
	{
		var model = new CreateTutorSubjectDto { TutorId = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.TutorId)
			.WithErrorMessage("Tutor ID is required");
	}

	[Fact]
	public void TutorId_WhenNegative_ResultError()
	{
		var model = new CreateTutorSubjectDto { TutorId = -1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.TutorId)
			.WithErrorMessage("Tutor ID must be a positive number");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(100)]
	[InlineData(999999)]
	public void TutorId_WhenValid_ResultOk(int tutorId)
	{
		var model = new CreateTutorSubjectDto
		{
			TutorId = tutorId,
			SubjectId = 1,
			LevelId = 1,
			HourlyRate = 100.50m
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.TutorId);
	}

	#endregion

	#region SubjectId Tests

	[Fact]
	public void SubjectId_WhenEmpty_ResultError()
	{
		var model = new CreateTutorSubjectDto { SubjectId = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.SubjectId)
			.WithErrorMessage("Subject ID is required");
	}

	[Fact]
	public void SubjectId_WhenNegative_ResultError()
	{
		var model = new CreateTutorSubjectDto { SubjectId = -1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.SubjectId)
			.WithErrorMessage("Subject ID must be a positive number");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(100)]
	[InlineData(999999)]
	public void SubjectId_WhenValid_ResultOk(int subjectId)
	{
		var model = new CreateTutorSubjectDto
		{
			TutorId = 1,
			SubjectId = subjectId,
			LevelId = 1,
			HourlyRate = 100.50m
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.SubjectId);
	}

	#endregion

	#region LevelId Tests

	[Fact]
	public void LevelId_WhenEmpty_ResultError()
	{
		var model = new CreateTutorSubjectDto { LevelId = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.LevelId)
			.WithErrorMessage("Level ID is required");
	}

	[Fact]
	public void LevelId_WhenNegative_ResultError()
	{
		var model = new CreateTutorSubjectDto { LevelId = -1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.LevelId)
			.WithErrorMessage("Level ID must be a positive number");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(100)]
	[InlineData(999999)]
	public void LevelId_WhenValid_ResultOk(int levelId)
	{
		var model = new CreateTutorSubjectDto
		{
			TutorId = 1,
			SubjectId = 1,
			LevelId = levelId,
			HourlyRate = 100.50m
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.LevelId);
	}

	#endregion

	#region HourlyRate Tests

	[Fact]
	public void HourlyRate_WhenZero_ResultError()
	{
		var model = new CreateTutorSubjectDto { HourlyRate = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.HourlyRate)
			.WithErrorMessage("Hourly rate is required");
	}

	[Fact]
	public void HourlyRate_WhenNegative_ResultError()
	{
		var model = new CreateTutorSubjectDto { HourlyRate = -10.50m };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.HourlyRate)
			.WithErrorMessage("Hourly rate must be a positive number");
	}

	[Fact]
	public void HourlyRate_WhenExceedsPrecision_ResultError()
	{
		var model = new CreateTutorSubjectDto { HourlyRate = 1234567.89m };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.HourlyRate)
			.WithErrorMessage("Hourly rate should consist of a maximum of 8 digits, of which no more than 2 can be decimal places");
	}

	[Fact]
	public void HourlyRate_WhenExceedsScale_ResultError()
	{
		var model = new CreateTutorSubjectDto { HourlyRate = 100.999m };
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
		var model = new CreateTutorSubjectDto
		{
			TutorId = 1,
			SubjectId = 1,
			LevelId = 1,
			HourlyRate = hourlyRate
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.HourlyRate);
	}

	#endregion
}