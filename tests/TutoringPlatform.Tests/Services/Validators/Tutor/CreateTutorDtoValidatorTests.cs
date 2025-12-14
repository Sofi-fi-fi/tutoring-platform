using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.Tutor;
using TutoringPlatform.Services.Validators.Tutor;

namespace TutoringPlatform.Tests.Services.Validators.Tutor;

public class CreateTutorDtoValidatorTests
{
	private readonly CreateTutordtovalidator _validator;

	public CreateTutorDtoValidatorTests()
	{
		_validator = new CreateTutordtovalidator();
	}

	#region CityId Tests

	[Fact]
	public void CityId_WhenNull_ResultOk()
	{
		var model = new CreateTutorDto
		{
			CityId = null,
			YearsExperience = 5,
			Education = "Igor Sikorsky KPI",
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.CityId);
	}

	[Fact]
	public void CityId_WhenZero_ResultError()
	{
		var model = new CreateTutorDto
		{
			CityId = 0,
			YearsExperience = 5,
			Education = "Igor Sikorsky KPI",
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.CityId)
			.WithErrorMessage("City ID must be a positive number");
	}

	[Fact]
	public void CityId_WhenNegative_ResultError()
	{
		var model = new CreateTutorDto
		{
			CityId = -1,
			YearsExperience = 5,
			Education = "Igor Sikorsky KPI",
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.CityId)
			.WithErrorMessage("City ID must be a positive number");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	public void CityId_WhenPositive_ResultOk(int cityId)
	{
		var model = new CreateTutorDto
		{
			CityId = cityId,
			YearsExperience = 5,
			Education = "Igor Sikorsky KPI",
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.CityId);
	}

	#endregion

	#region YearsExperience Tests

	[Fact]
	public void YearsExperience_WhenZero_ResultError()
	{
		var model = new CreateTutorDto
		{
			YearsExperience = 0,
			Education = "Igor Sikorsky KPI",
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.YearsExperience)
			.WithErrorMessage("Years of experience must be a positive number");
	}

	[Fact]
	public void YearsExperience_WhenNegative_ResultError()
	{
		var model = new CreateTutorDto
		{
			YearsExperience = -1,
			Education = "Igor Sikorsky KPI",
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.YearsExperience)
			.WithErrorMessage("Years of experience must be a positive number");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(10)]
	[InlineData(20)]
	[InlineData(30)]
	public void YearsExperience_WhenPositive_ResultOk(short years)
	{
		var model = new CreateTutorDto
		{
			YearsExperience = years,
			Education = "Igor Sikorsky KPI",
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.YearsExperience);
	}

	#endregion

	#region Education Tests

	[Fact]
	public void Education_WhenEmpty_ResultError()
	{
		var model = new CreateTutorDto
		{
			Education = string.Empty,
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Education)
			.WithErrorMessage("Education is required");
	}

	[Fact]
	public void Education_WhenNull_ResultError()
	{
		var model = new CreateTutorDto
		{
			Education = null!,
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Education);
	}

	[Theory]
	[InlineData("Igor Sikorsky KPI in Mathematics")]
	[InlineData("Professor at Igor Sikorsky KPI")]
	[InlineData("PhD")]
	[InlineData("Self-taught")]
	[InlineData("Diploma Velicaga Pragramista")]
	public void Education_WhenValid_ResultOk(string education)
	{
		var model = new CreateTutorDto
		{
			Education = education,
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Education);
	}

	#endregion

	#region Availability Tests

	[Fact]
	public void Availability_WhenBothFalse_ResultError()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = false,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x)
			.WithErrorMessage("At least one of online or offline availability must be true");
	}

	[Fact]
	public void Availability_WhenOnlineOnly_ResultOk()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x);
	}

	[Fact]
	public void Availability_WhenOfflineOnly_ResultOk()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = false,
			OfflineAvailable = true,
			Address = "41 Polytechnichna Street"
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x);
	}

	[Fact]
	public void Availability_WhenBothTrue_ResultOk()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = true,
			Address = "41 Polytechnichna Street"
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x);
	}

	#endregion

	#region AboutMe Tests

	[Fact]
	public void AboutMe_WhenNull_ResultOk()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false,
			AboutMe = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.AboutMe);
	}

	[Fact]
	public void AboutMe_WhenEmpty_ResultOk()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false,
			AboutMe = string.Empty
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.AboutMe);
	}

	[Fact]
	public void AboutMe_WhenExceedsMaxLength_ResultError()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false,
			AboutMe = new string('a', 2001)
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.AboutMe)
			.WithErrorMessage("About Me section cannot exceed 2000 characters");
	}

	[Theory]
	[InlineData("I am an experienced tutor")]
	[InlineData("I specialize in mathematics and physics for students")]
	[InlineData("")]
	public void AboutMe_WhenValidLength_ResultOk(string aboutMe)
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false,
			AboutMe = aboutMe
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.AboutMe);
	}

	#endregion

	#region Address Tests

	[Fact]
	public void Address_WhenNull_AndOfflineAvailable_ResultError()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = false,
			OfflineAvailable = true,
			Address = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Address)
			.WithErrorMessage("Address is required when offline availability is true");
	}

	[Fact]
	public void Address_WhenEmpty_AndOfflineAvailable_ResultError()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = false,
			OfflineAvailable = true,
			Address = string.Empty
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Address)
			.WithErrorMessage("Address is required when offline availability is true");
	}

	[Fact]
	public void Address_WhenNull_AndOnlineOnly_ResultOk()
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = true,
			OfflineAvailable = false,
			Address = null
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Address);
	}

	[Theory]
	[InlineData("41 Polytechnichna Street")]
	[InlineData(" 37, Beresteysky Avenue")]
	[InlineData("Olena Teliga Square")]
	public void Address_WhenValid_AndOfflineAvailable_ResultOk(string address)
	{
		var model = new CreateTutorDto
		{
			Education = "Igor Sikorsky KPI",
			YearsExperience = 5,
			OnlineAvailable = false,
			OfflineAvailable = true,
			Address = address
		};
		
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Address);
	}

	#endregion
}
