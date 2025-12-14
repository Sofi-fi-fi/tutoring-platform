using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.Subject;
using TutoringPlatform.Services.Validators.Subject;

namespace TutoringPlatform.Tests.Services.Validators.Subject;

public class UpdateSubjectDtoValidatorTests
{
	private readonly UpdateSubjectDtoValidator _validator;

	public UpdateSubjectDtoValidatorTests()
	{
		_validator = new UpdateSubjectDtoValidator();
	}

	#region Name Tests

	[Fact]
	public void Name_WhenEmpty_ResultError()
	{
		var model = new UpdateSubjectDto { Name = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Subject name is required");
	}

	[Fact]
	public void Name_WhenNull_ResultError()
	{
		var model = new UpdateSubjectDto { Name = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name);
	}

	[Fact]
	public void Name_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateSubjectDto { Name = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Subject name cannot exceed 100 characters");
	}

	[Theory]
	[InlineData("Mathematics")]
	[InlineData("Математика")]
	[InlineData("English Language")]
	[InlineData("Англійська мова")]
	[InlineData("Computer Science")]
	[InlineData("Інформатика")]
	[InlineData("Physics & Chemistry")]
	[InlineData("Фізика та Хімія")]
	[InlineData("A")]
	[InlineData("Subject with 100 characters - this is a very long subject name that should still be valid because")]
	public void Name_WhenValid_ResultOk(string name)
	{
		var model = new UpdateSubjectDto
		{
			Name = name,
			Category = "Science"
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Name);
	}

	#endregion

	#region Category Tests

	[Fact]
	public void Category_WhenEmpty_ResultError()
	{
		var model = new UpdateSubjectDto { Category = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Category)
			.WithErrorMessage("Subject category is required");
	}

	[Fact]
	public void Category_WhenNull_ResultError()
	{
		var model = new UpdateSubjectDto { Category = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Category);
	}

	[Fact]
	public void Category_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateSubjectDto { Category = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Category)
			.WithErrorMessage("Subject category cannot exceed 100 characters");
	}

	[Theory]
	[InlineData("Science")]
	[InlineData("Природничі науки")]
	[InlineData("Mathematics")]
	[InlineData("Математика")]
	[InlineData("Languages")]
	[InlineData("Мови")]
	[InlineData("Arts & Humanities")]
	[InlineData("Мистецтво та гуманітарні науки")]
	[InlineData("Technology")]
	[InlineData("Технології")]
	public void Category_WhenValid_ResultOk(string category)
	{
		var model = new UpdateSubjectDto
		{
			Name = "Test Subject",
			Category = category
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Category);
	}

	#endregion

	#region Description Tests

	[Fact]
	public void Description_WhenNull_ResultOk()
	{
		var model = new UpdateSubjectDto
		{
			Name = "Mathematics",
			Category = "Science",
			Description = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Description);
	}

	[Fact]
	public void Description_WhenEmpty_ResultOk()
	{
		var model = new UpdateSubjectDto
		{
			Name = "Mathematics",
			Category = "Science",
			Description = string.Empty
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Description);
	}

	[Fact]
	public void Description_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateSubjectDto 
		{ 
			Name = "Mathematics",
			Category = "Science",
			Description = new string('a', 501) 
		};
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Description)
			.WithErrorMessage("Subject description cannot exceed 500 characters");
	}

	[Theory]
	[InlineData("Basic mathematics course")]
	[InlineData("Базовий курс математики")]
	[InlineData("This is a comprehensive subject covering various topics")]
	[InlineData("Комплексний предмет, що охоплює різні теми")]
	[InlineData("A")]
	public void Description_WhenValid_ResultOk(string description)
	{
		var model = new UpdateSubjectDto
		{
			Name = "Mathematics",
			Category = "Science",
			Description = description
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Description);
	}

	[Fact]
	public void Description_WhenMaxLength_ResultOk()
	{
		var model = new UpdateSubjectDto
		{
			Name = "Mathematics",
			Category = "Science",
			Description = new string('a', 500)
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Description);
	}

	#endregion
}