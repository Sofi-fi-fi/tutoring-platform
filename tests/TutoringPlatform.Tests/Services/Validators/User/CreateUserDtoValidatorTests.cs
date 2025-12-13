using FluentValidation.TestHelper;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Services.DTOs.User;
using TutoringPlatform.Services.Validators.User;

namespace TutoringPlatform.Tests.Services.Validators.User;

public class CreateUserDtoValidatorTests
{
	private readonly CreateUserDtoValidator _validator;

	public CreateUserDtoValidatorTests()
	{
		_validator = new CreateUserDtoValidator();
	}

	#region FirstName Tests

	[Fact]
	public void FirstName_WhenEmpty_ResultError()
	{
		var model = new CreateUserDto { FirstName = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.FirstName)
			.WithErrorMessage("First name is required");
	}

	[Fact]
	public void FirstName_WhenNull_ResultError()
	{
		var model = new CreateUserDto { FirstName = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.FirstName);
	}

	[Fact]
	public void FirstName_WhenExceedsMaxLength_ResultError()
	{
		var model = new CreateUserDto { FirstName = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.FirstName)
			.WithErrorMessage("First name cannot exceed 100 characters");
	}

	[Theory]
	[InlineData("Studentik123")]
	[InlineData("Студентік123")]
	[InlineData("Studentik@")]
	[InlineData("Студентік@")]
	[InlineData("Studentik#Studentovych")]
	[InlineData("Студентік#Студентович")]
	[InlineData("Studentik Studentovych")]
	[InlineData("Студентік Студентович")]
	public void FirstName_WhenContainsInvalidCharacters_ResultError(string firstName)
	{
		var model = new CreateUserDto { FirstName = firstName };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.FirstName)
			.WithErrorMessage("First name can only contain letters, apostrophes, and hyphens");
	}

	[Theory]
	[InlineData("Studentik")]
	[InlineData("Студентік")]
	[InlineData("Studentik-Junior")]
	[InlineData("Студентік-Молодший")]
	[InlineData("O'Studentik")]
	[InlineData("О'Студентік")]
	[InlineData("La-Studentik")]
	[InlineData("Ла-Студентік")]
	[InlineData("Studentik-Studentov")]
	[InlineData("Студентік-Студентов")]
	public void FirstName_WhenValid_ResultOk(string firstName)
	{
		var model = new CreateUserDto
		{
			FirstName = firstName,
			LastName = "Studentovych",
			Email = "studentik@test.com",
			UserType = UserType.Student
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
	}

	#endregion

	#region LastName Tests

	[Fact]
	public void LastName_WhenEmpty_ResultError()
	{
		var model = new CreateUserDto { LastName = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.LastName)
			.WithErrorMessage("Last name is required");
	}

	[Fact]
	public void LastName_WhenNull_ResultError()
	{
		var model = new CreateUserDto { LastName = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.LastName);
	}

	[Fact]
	public void LastName_WhenExceedsMaxLength_ResultError()
	{
		var model = new CreateUserDto { LastName = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.LastName)
			.WithErrorMessage("Last name cannot exceed 100 characters");
	}

	[Theory]
	[InlineData("Studentovych123")]
	[InlineData("Студентович123")]
	[InlineData("Studentovych@")]
	[InlineData("Студентович@")]
	[InlineData("Studentovych#Studentovych")]
	[InlineData("Студентович#Студентович")]
	[InlineData("Studentovych Studentovych")]
	[InlineData("Студентович Студентович")]
	public void LastName_WhenContainsInvalidCharacters_ResultError(string lastName)
	{
		var model = new CreateUserDto { LastName = lastName };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.LastName)
			.WithErrorMessage("Last name can only contain letters, apostrophes, and hyphens");
	}

	[Theory]
	[InlineData("Studentovych")]
	[InlineData("Студентович")]
	[InlineData("O'Studentovych")]
	[InlineData("О'Студентович")]
	[InlineData("Studentovych-Studentie")]
	[InlineData("Студентович-Студентовіє")]
	[InlineData("MacStudentov")]
	[InlineData("МакСтудентов")]
	public void LastName_WhenValid_ResultOk(string lastName)
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = lastName,
			Email = "studentik@test.com",
			UserType = UserType.Student
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.LastName);
	}

	#endregion

	#region Email Tests

	[Fact]
	public void Email_WhenEmpty_ResultError()
	{
		var model = new CreateUserDto { Email = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Email)
			.WithErrorMessage("Email is required");
	}

	[Fact]
	public void Email_WhenNull_ResultError()
	{
		var model = new CreateUserDto { Email = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Email);
	}

	[Theory]
	[InlineData("studentikemail")]
	[InlineData("@studentik.com")]
	[InlineData("something.at.com")]
	[InlineData("studentik@")]
	public void Email_WhenInvalidFormat_ResultError(string email)
	{
		var model = new CreateUserDto { Email = email };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Email)
			.WithErrorMessage("Invalid email format");
	}

	[Fact]
	public void Email_WhenExceedsMaxLength_ResultError()
	{
		var model = new CreateUserDto { Email = new string('a', 246) + "@email.com" };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Email)
			.WithErrorMessage("Email cannot exceed 255 characters");
	}

	[Theory]
	[InlineData("studentik@test.com")]
	[InlineData("test.studentik@domain.co.ua")]
	[InlineData("firstname.lastname@company.com")]
	[InlineData("user+tag@test.com")]
	public void Email_WhenValid_ResultOk(string email)
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = email,
			UserType = UserType.Student
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Email);
	}

	#endregion

	#region Phone Tests

	[Fact]
	public void Phone_WhenNull_ResultOk()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			Phone = null,
			UserType = UserType.Student
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Phone);
	}

	[Fact]
	public void Phone_WhenEmpty_ResultOk()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			Phone = string.Empty,
			UserType = UserType.Student
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Phone);
	}

	[Fact]
	public void Phone_WhenExceedsMaxLength_ResultError()
	{
		var model = new CreateUserDto { Phone = new string('1', 21) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Phone)
			.WithErrorMessage("Phone number cannot exceed 20 characters");
	}

	[Theory]
	[InlineData("123")]
	[InlineData("12345678")]
	[InlineData("abcdefghij")]
	[InlineData("(555) 123-4567")]
	[InlineData("555-123-4567")]
	public void Phone_WhenInvalidFormat_ResultError(string phone)
	{
		var model = new CreateUserDto { Phone = phone };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Phone)
			.WithErrorMessage("Invalid phone number format");
	}

	[Theory]
	[InlineData("1234567890")]
	[InlineData("12345678901")]
	[InlineData("+1234567890")]
	[InlineData("+12345678901234")]
	[InlineData("15551234567")]
	public void Phone_WhenValid_ResultOk(string phone)
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			Phone = phone,
			UserType = UserType.Student
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Phone);
	}

	#endregion

	#region UserType Tests

	[Fact]
	public void UserType_WhenValid_ResultOk()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			UserType = UserType.Student
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.UserType);
	}

	[Fact]
	public void UserType_WhenInvalid_ResultError()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			UserType = (UserType)999
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.UserType)
			.WithErrorMessage("Invalid user type");
	}

	#endregion

	#region DateOfBirth Tests

	[Fact]
	public void DateOfBirth_WhenNull_ResultOk()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			UserType = UserType.Student,
			DateOfBirth = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
	}

	[Fact]
	public void DateOfBirth_WhenInPast_ResultOk()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			UserType = UserType.Student,
			DateOfBirth = DateTime.Now.AddYears(-20)
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
	}

	[Fact]
	public void DateOfBirth_WhenInFuture_ResultError()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			UserType = UserType.Student,
			DateOfBirth = DateTime.Now.AddDays(1)
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
			.WithErrorMessage("Date of birth must be in the past");
	}

	[Fact]
	public void DateOfBirth_WhenToday_ResultError()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			UserType = UserType.Student,
			DateOfBirth = DateTime.Now
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
			.WithErrorMessage("Date of birth must be in the past");
	}

	#endregion

	#region Complete Object Tests

	[Fact]
	public void Validate_WhenAllFieldsValid_ResultOk()
	{
		var model = new CreateUserDto
		{
			FirstName = "Studentik",
			LastName = "Studentovych",
			Email = "studentik@test.com",
			Phone = "+12345678901",
			UserType = UserType.Student,
			DateOfBirth = DateTime.Now.AddYears(-25)
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void Validate_WhenMultipleFieldsInvalid_ResultMultipleErrors()
	{
		var model = new CreateUserDto
		{
			FirstName = string.Empty,
			LastName = string.Empty,
			Email = "invalid-email",
			Phone = "123",
			UserType = (UserType)999,
			DateOfBirth = DateTime.Now.AddDays(1)
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.FirstName);
		result.ShouldHaveValidationErrorFor(x => x.LastName);
		result.ShouldHaveValidationErrorFor(x => x.Email);
		result.ShouldHaveValidationErrorFor(x => x.Phone);
		result.ShouldHaveValidationErrorFor(x => x.UserType);
		result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
	}

	#endregion
}