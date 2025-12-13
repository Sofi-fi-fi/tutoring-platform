using FluentValidation.TestHelper;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Services.DTOs.Booking;
using TutoringPlatform.Services.Validators.Booking;

namespace TutoringPlatform.Tests.Services.Validators.Booking;

public class CreateBookingDtoValidatorTests
{
	private readonly CreateBookingDtoValidator _validator;

	public CreateBookingDtoValidatorTests()
	{
		_validator = new CreateBookingDtoValidator();
	}

	#region StudentId Tests

	[Fact]
	public void StudentId_WhenZero_ResultError()
	{
		var model = new CreateBookingDto { StudentId = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.StudentId)
			.WithErrorMessage("StudentId must be a positive integer");
	}

	[Fact]
	public void StudentId_WhenNegative_ResultError()
	{
		var model = new CreateBookingDto { StudentId = -1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.StudentId)
			.WithErrorMessage("StudentId must be a positive integer");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(999999)]
	public void StudentId_WhenPositive_ResultOk(int studentId)
	{
		var model = new CreateBookingDto
		{
			StudentId = studentId,
			TutorSubjectId = 1,
			ScheduleId = 1,
			Format = BookingFormat.Online
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.StudentId);
	}

	#endregion

	#region TutorSubjectId Tests

	[Fact]
	public void TutorSubjectId_WhenZero_ResultError()
	{
		var model = new CreateBookingDto { TutorSubjectId = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.TutorSubjectId)
			.WithErrorMessage("TutorSubjectId must be a positive integer");
	}

	[Fact]
	public void TutorSubjectId_WhenNegative_ResultError()
	{
		var model = new CreateBookingDto { TutorSubjectId = -1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.TutorSubjectId)
			.WithErrorMessage("TutorSubjectId must be a positive integer");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(999999)]
	public void TutorSubjectId_WhenPositive_ResultOk(int tutorSubjectId)
	{
		var model = new CreateBookingDto
		{
			StudentId = 1,
			TutorSubjectId = tutorSubjectId,
			ScheduleId = 1,
			Format = BookingFormat.Online
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.TutorSubjectId);
	}

	#endregion

	#region ScheduleId Tests

	[Fact]
	public void ScheduleId_WhenZero_ResultError()
	{
		var model = new CreateBookingDto { ScheduleId = 0 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.ScheduleId)
			.WithErrorMessage("ScheduleId must be a positive integer");
	}

	[Fact]
	public void ScheduleId_WhenNegative_ResultError()
	{
		var model = new CreateBookingDto { ScheduleId = -1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.ScheduleId)
			.WithErrorMessage("ScheduleId must be a positive integer");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(999999)]
	public void ScheduleId_WhenPositive_ResultOk(int scheduleId)
	{
		var model = new CreateBookingDto
		{
			StudentId = 1,
			TutorSubjectId = 1,
			ScheduleId = scheduleId,
			Format = BookingFormat.Online
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.ScheduleId);
	}

	#endregion

	#region Format Tests

	[Fact]
	public void Format_WhenInvalidValue_ResultError()
	{
		var model = new CreateBookingDto { Format = (BookingFormat)999 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Format)
			.WithErrorMessage("Invalid booking format");
	}

	[Theory]
	[InlineData(BookingFormat.Online)]
	[InlineData(BookingFormat.Offline)]
	public void Format_WhenValid_ResultOk(BookingFormat format)
	{
		var model = new CreateBookingDto
		{
			StudentId = 1,
			TutorSubjectId = 1,
			ScheduleId = 1,
			Format = format
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Format);
	}

	#endregion
}