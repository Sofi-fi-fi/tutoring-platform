using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.TeachingLevel;
using TutoringPlatform.Services.Validators.TeachingLevel;

namespace TutoringPlatform.Tests.Services.Validators.TeachingLevel;

public class UpdateTeachingLevelDtoValidatorTests
{
	private readonly UpdateTeachingLevelDtoValidator _validator;

	public UpdateTeachingLevelDtoValidatorTests()
	{
		_validator = new UpdateTeachingLevelDtoValidator();
	}

	#region Name Tests

	[Fact]
	public void Name_WhenEmpty_ResultError()
	{
		var model = new UpdateTeachingLevelDto { Name = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Teaching level name is required");
	}

	[Fact]
	public void Name_WhenNull_ResultError()
	{
		var model = new UpdateTeachingLevelDto { Name = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name);
	}

	[Fact]
	public void Name_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateTeachingLevelDto { Name = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Teaching level name cannot exceed 100 characters");
	}

	[Theory]
	[InlineData("123")]
	[InlineData("456789")]
	[InlineData("!@#$%")]
	[InlineData("123-456")]
	[InlineData("---")]
	[InlineData(" ")]
	public void Name_WhenContainsNoLetters_ResultError(string name)
	{
		var model = new UpdateTeachingLevelDto { Name = name };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Teaching level name must contain at least one letter");
	}

	[Theory]
	[InlineData("Elementary")]
	[InlineData("Middle School")]
	[InlineData("High School")]
	[InlineData("University")]
	[InlineData("Початкова школа")]
	[InlineData("Середня школа")]
	[InlineData("Старші класи")]
	[InlineData("Університет")]
	public void Name_WhenValid_ResultOk(string name)
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = name,
			Position = 1
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Name);
	}

	#endregion

	#region Position Tests

	[Fact]
	public void Position_WhenZero_ResultError()
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = "Elementary",
			Position = 0
		};
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Position)
			.WithErrorMessage("Position must be a positive number");
	}

	[Fact]
	public void Position_WhenNegative_ResultError()
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = "Elementary",
			Position = -1
		};
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Position)
			.WithErrorMessage("Position must be a positive number");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(32767)]
	public void Position_WhenPositive_ResultOk(short position)
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = "Elementary",
			Position = position
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Position);
	}

	#endregion

	#region Description Tests

	[Fact]
	public void Description_WhenNull_ResultOk()
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = "Elementary",
			Position = 1,
			Description = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Description);
	}

	[Fact]
	public void Description_WhenEmpty_ResultOk()
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = "Elementary",
			Position = 1,
			Description = string.Empty
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Description);
	}

	[Fact]
	public void Description_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = "Elementary",
			Position = 1,
			Description = new string('a', 501)
		};
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Description)
			.WithErrorMessage("Description cannot exceed 500 characters");
	}

	[Theory]
	[InlineData("Basic level education")]
	[InlineData("For students in grades 1-4")]
	[InlineData("1-9 класи")]
	[InlineData("Для учнів 1-4 класів")]
	public void Description_WhenValid_ResultOk(string description)
	{
		var model = new UpdateTeachingLevelDto
		{
			Name = "Elementary",
			Position = 1,
			Description = description
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Description);
	}

	#endregion
}