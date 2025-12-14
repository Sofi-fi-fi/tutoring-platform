using FluentValidation.TestHelper;
using TutoringPlatform.Services.DTOs.Schedule;
using TutoringPlatform.Services.Validators.Schedule;

namespace TutoringPlatform.Tests.Services.Validators.Schedule;

public class UpdateScheduleDtoValidatorTests
{
	private readonly UpdateScheduleDtoValidator _validator;

	public UpdateScheduleDtoValidatorTests()
	{
		_validator = new UpdateScheduleDtoValidator();
	}

	#region Date Tests

	[Fact]
	public void Date_WhenInPast_ResultError()
	{
		var model = new UpdateScheduleDto { Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)) };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Date)
			.WithErrorMessage("Date cannot be in the past");
	}

	[Fact]
	public void Date_WhenToday_ResultOk()
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today),
			StartTime = new TimeOnly(10, 0),
			EndTime = new TimeOnly(11, 0)
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Date);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(7)]
	[InlineData(30)]
	[InlineData(365)]
	public void Date_WhenInFuture_ResultOk(int daysFromToday)
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today.AddDays(daysFromToday)),
			StartTime = new TimeOnly(10, 0),
			EndTime = new TimeOnly(11, 0)
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.Date);
	}

	#endregion

	#region StartTime Tests

	[Fact]
	public void StartTime_WhenEmpty_ResultError()
	{
		var model = new UpdateScheduleDto { StartTime = default };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.StartTime)
			.WithErrorMessage("StartTime is required");
	}

	[Theory]
	[InlineData(8, 0)]
	[InlineData(12, 30)]
	[InlineData(23, 59)]
	public void StartTime_WhenValidTime_ResultOk(int hour, int minute)
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
			StartTime = new TimeOnly(hour, minute),
			EndTime = new TimeOnly(hour, minute).AddHours(1)
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.StartTime);
	}

	#endregion

	#region EndTime Tests

	[Fact]
	public void EndTime_WhenEmpty_ResultError()
	{
		var model = new UpdateScheduleDto { EndTime = default };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.EndTime)
			.WithErrorMessage("EndTime is required");
	}

	[Theory]
	[InlineData(1, 0)]
	[InlineData(9, 0)]
	[InlineData(13, 30)]
	[InlineData(23, 59)]
	public void EndTime_WhenValidTime_ResultOk(int hour, int minute)
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
			StartTime = new TimeOnly(hour, minute).AddHours(-1),
			EndTime = new TimeOnly(hour, minute)
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x.EndTime);
	}

	#endregion

	#region Time Range Tests

	[Fact]
	public void TimeRange_WhenEndTimeBeforeStartTime_ResultError()
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
			StartTime = new TimeOnly(11, 0),
			EndTime = new TimeOnly(10, 0)
		};

		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x)
			.WithErrorMessage("EndTime must be later than StartTime");
	}

	[Fact]
	public void TimeRange_WhenEndTimeEqualsStartTime_ResultError()
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
			StartTime = new TimeOnly(10, 0),
			EndTime = new TimeOnly(10, 0)
		};

		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x)
			.WithErrorMessage("EndTime must be later than StartTime");
	}

	[Fact]
	public void TimeRange_WhenDurationIsNot60Minutes_ResultError()
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
			StartTime = new TimeOnly(10, 0),
			EndTime = new TimeOnly(10, 30)
		};

		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x)
			.WithErrorMessage("The duration between StartTime and EndTime must be exactly 60 minutes");
	}

	[Theory]
	[InlineData(10, 0, 11, 0)]
	[InlineData(9, 30, 10, 30)]
	[InlineData(14, 15, 15, 15)]
	[InlineData(0, 0, 1, 0)]
	[InlineData(22, 0, 23, 0)]
	public void TimeRange_WhenDurationIs60Minutes_ResultOk(int startHour, int startMinute, int endHour, int endMinute)
	{
		var model = new UpdateScheduleDto
		{
			Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
			StartTime = new TimeOnly(startHour, startMinute),
			EndTime = new TimeOnly(endHour, endMinute)
		};

		var result = _validator.TestValidate(model);
		result.ShouldNotHaveValidationErrorFor(x => x);
	}

	#endregion
}