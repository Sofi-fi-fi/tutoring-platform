using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Services.DTOs.Booking;

public class CreateBookingDto
{
	public int StudentId { get; set; }
	public int TutorSubjectId { get; set; }
	public int ScheduleId { get; set; }
	public BookingFormat Format { get; set; }
}