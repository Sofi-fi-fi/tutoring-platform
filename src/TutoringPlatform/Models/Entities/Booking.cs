using System;
using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Models.Entities;

public class Booking
{
	public int BookingId { get; set; }
	public int StudentId { get; set; }
	public int TutorSubjectId { get; set; }
	public int ScheduleId { get; set; }
	public BookingFormat Format { get; set; }
	public BookingStatus Status { get; set; } = BookingStatus.Pending;
	public DateTime CreatedAt { get; set; }

	public virtual Student Student { get; set; } = null!; 
	public virtual TutorSubject TutorSubject { get; set; } = null!;
	public virtual Schedule Schedule { get; set; } = null!;
	public virtual Review? Review { get; set; }
}
