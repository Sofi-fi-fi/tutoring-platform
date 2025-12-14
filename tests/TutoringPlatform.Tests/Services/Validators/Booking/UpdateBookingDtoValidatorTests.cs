using FluentValidation.TestHelper;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Services.DTOs.Booking;
using TutoringPlatform.Services.Validators.Booking;

namespace TutoringPlatform.Tests.Services.Validators.Booking;

public class UpdateBookingDtoValidatorTests
{
	private readonly UpdateBookingDtoValidator _validator;

	public UpdateBookingDtoValidatorTests()
	{
		_validator = new UpdateBookingDtoValidator();
	}

	#region Status Tests

	[Fact]
	public void Status_WhenInvalidValue_ResultError()
	{
		var model = new UpdateBookingDto { Status = (BookingStatus)999 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Status)
			.WithErrorMessage("Invalid booking status");
	}

	[Theory]
	[InlineData(BookingStatus.Pending)]
	[InlineData(BookingStatus.Confirmed)]
	[InlineData(BookingStatus.Completed)]
	[InlineData(BookingStatus.Cancelled)]
	public void Status_WhenValid_ResultOk(BookingStatus status)
	{
		var model = new UpdateBookingDto
		{
			Status = status
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Status);
	}

	#endregion
}