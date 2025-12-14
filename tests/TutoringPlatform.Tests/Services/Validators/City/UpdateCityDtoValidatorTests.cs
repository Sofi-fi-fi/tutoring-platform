using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.City;
using TutoringPlatform.Services.Validators.City;

namespace TutoringPlatform.Tests.Services.Validators.City;

public class UpdateCityDtoValidatorTests
{
	private readonly UpdateCityDtoValidator _validator;

	public UpdateCityDtoValidatorTests()
	{
		_validator = new UpdateCityDtoValidator();
	}

	#region Name Tests

	[Fact]
	public void Name_WhenEmpty_ResultError()
	{
		var model = new UpdateCityDto { Name = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("City name is required");
	}

	[Fact]
	public void Name_WhenNull_ResultError()
	{
		var model = new UpdateCityDto { Name = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name);
	}

	[Fact]
	public void Name_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateCityDto { Name = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("City name must not exceed 100 characters");
	}

	[Theory]
	[InlineData("Kyiv123")]
	[InlineData("Київ123")]
	[InlineData("Kyiv@")]
	[InlineData("Київ@")]
	[InlineData("Kyiv#City")]
	[InlineData("Київ#Місто")]
	[InlineData("Kyiv_City")]
	[InlineData("Київ_Місто")]
	public void Name_WhenContainsInvalidCharacters_ResultError(string name)
	{
		var model = new UpdateCityDto { Name = name };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("City name contains invalid characters");
	}

	[Theory]
	[InlineData("Kyiv")]
	[InlineData("Київ")]
	[InlineData("Lviv")]
	[InlineData("Львів")]
	[InlineData("Dnipro")]
	[InlineData("Дніпро")]
	[InlineData("Ivano-Frankivsk")]
	[InlineData("Івано-Франківськ")]
	[InlineData("Kam'yanets-Podilskyi")]
	[InlineData("Кам'янець-Подільський")]
	public void Name_WhenValid_ResultOk(string name)
	{
		var model = new UpdateCityDto
		{
			Name = name,
			Country = "Ukraine"
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Name);
	}

	#endregion

	#region Region Tests

	[Fact]
	public void Region_WhenNull_ResultOk()
	{
		var model = new UpdateCityDto
		{
			Name = "Kyiv",
			Region = null,
			Country = "Ukraine"
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Region);
	}

	[Fact]
	public void Region_WhenEmpty_ResultOk()
	{
		var model = new UpdateCityDto
		{
			Name = "Kyiv",
			Region = string.Empty,
			Country = "Ukraine"
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Region);
	}

	[Fact]
	public void Region_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateCityDto { Region = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Region)
			.WithErrorMessage("Region must not exceed 100 characters");
	}

	[Theory]
	[InlineData("Kyiv Oblast123")]
	[InlineData("Київська область123")]
	[InlineData("Kyiv Oblast@")]
	[InlineData("Київська область@")]
	[InlineData("Kyiv#Oblast")]
	[InlineData("Київська#область")]
	[InlineData("Kyiv_Oblast")]
	[InlineData("Київська_область")]
	public void Region_WhenContainsInvalidCharacters_ResultError(string region)
	{
		var model = new UpdateCityDto { Region = region };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Region)
			.WithErrorMessage("Region contains invalid characters");
	}

	[Theory]
	[InlineData("Kyiv Oblast")]
	[InlineData("Київська область")]
	[InlineData("Lviv Oblast")]
	[InlineData("Львівська область")]
	[InlineData("Ivano-Frankivsk Oblast")]
	[InlineData("Івано-Франківська область")]
	public void Region_WhenValid_ResultOk(string region)
	{
		var model = new UpdateCityDto
		{
			Name = "Kyiv",
			Region = region,
			Country = "Ukraine"
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Region);
	}

	#endregion

	#region Country Tests

	[Fact]
	public void Country_WhenEmpty_ResultError()
	{
		var model = new UpdateCityDto { Country = string.Empty };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Country)
			.WithErrorMessage("Country is required");
	}

	[Fact]
	public void Country_WhenNull_ResultError()
	{
		var model = new UpdateCityDto { Country = null! };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Country);
	}

	[Fact]
	public void Country_WhenExceedsMaxLength_ResultError()
	{
		var model = new UpdateCityDto { Country = new string('a', 101) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Country)
			.WithErrorMessage("Country must not exceed 100 characters");
	}

	[Theory]
	[InlineData("Ukraine123")]
	[InlineData("Україна123")]
	[InlineData("Ukraine@")]
	[InlineData("Україна@")]
	[InlineData("Ukraine#Country")]
	[InlineData("Україна#Країна")]
	[InlineData("Ukraine_Country")]
	[InlineData("Україна_Країна")]
	public void Country_WhenContainsInvalidCharacters_ResultError(string country)
	{
		var model = new UpdateCityDto { Country = country };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Country)
			.WithErrorMessage("Country contains invalid characters");
	}

	[Theory]
	[InlineData("Ukraine")]
	[InlineData("Україна")]
	public void Country_WhenValid_ResultOk(string country)
	{
		var model = new UpdateCityDto
		{
			Name = "Kyiv",
			Country = country
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Country);
	}

	#endregion
}