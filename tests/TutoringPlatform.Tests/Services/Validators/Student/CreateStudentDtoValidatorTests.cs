using FluentValidation.TestHelper;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Services.DTOs.User;
using TutoringPlatform.Services.DTOs.Student;
using TutoringPlatform.Services.Validators.Student;

namespace TutoringPlatform.Tests.Services.Validators.Student;

public class CreateStudentDtoValidatorTests
{
	private readonly CreateStudentDtoValidator _validator;

	public CreateStudentDtoValidatorTests()
	{
		_validator = new CreateStudentDtoValidator();
	}

	#region User Tests

	[Fact]
	public void User_WhenNull_ResultError()
	{
		var model = new CreateStudentDto { User = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.User)
			.WithErrorMessage("User is required");
	}

	[Fact]
	public void User_WhenValid_ResultOk()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			}
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.User);
	}

	[Fact]
	public void User_WhenInvalid_ResultError()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = string.Empty,
				LastName = string.Empty,
				Email = "invalid-email",
				UserType = UserType.Student
			}
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor("User.FirstName");
		result.ShouldHaveValidationErrorFor("User.LastName");
		result.ShouldHaveValidationErrorFor("User.Email");
	}

	#endregion

	#region CityId Tests

	[Fact]
	public void CityId_WhenNull_ResultOk()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			CityId = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.CityId);
	}

	[Fact]
	public void CityId_WhenZero_ResultError()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			CityId = 0
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.CityId)
			.WithErrorMessage("CityId must be greater than 0");
	}

	[Fact]
	public void CityId_WhenNegative_ResultError()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			CityId = -1
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.CityId)
			.WithErrorMessage("CityId must be greater than 0");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void CityId_WhenValid_ResultOk(int cityId)
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			CityId = cityId
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.CityId);
	}

	#endregion

	#region SchoolGrade Tests

	[Fact]
	public void SchoolGrade_WhenNull_ResultOk()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			SchoolGrade = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.SchoolGrade);
	}

	[Fact]
	public void SchoolGrade_WhenZero_ResultError()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			SchoolGrade = 0
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.SchoolGrade)
			.WithErrorMessage("SchoolGrade must be between 1 and 11");
	}

	[Fact]
	public void SchoolGrade_WhenNegative_ResultError()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			SchoolGrade = -1
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.SchoolGrade)
			.WithErrorMessage("SchoolGrade must be between 1 and 11");
	}

	[Fact]
	public void SchoolGrade_WhenGreaterThan11_ResultError()
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			SchoolGrade = 12
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.SchoolGrade)
			.WithErrorMessage("SchoolGrade must be between 1 and 11");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(9)]
	[InlineData(11)]
	public void SchoolGrade_WhenValid_ResultOk(short schoolGrade)
	{
		var model = new CreateStudentDto
		{
			User = new CreateUserDto
			{
				FirstName = "Studentik",
				LastName = "Studentovych",
				Email = "studentik@test.com",
				UserType = UserType.Student
			},
			SchoolGrade = schoolGrade
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.SchoolGrade);
	}

	#endregion
}