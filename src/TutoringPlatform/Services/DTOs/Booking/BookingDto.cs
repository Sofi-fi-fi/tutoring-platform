using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Services.DTOs.Booking;

public class BookingDto
{
	public int BookingId { get; set; }
	public int StudentId { get; set; }
	public int TutorSubjectId { get; set; }
	public int ScheduleId { get; set; }
	public BookingFormat Format { get; set; }
	public BookingStatus Status { get; set; }
	public DateTime CreatedAt { get; set; }	
}