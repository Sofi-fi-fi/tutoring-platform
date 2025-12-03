using System;


namespace TutoringPlatform.Models.Entities;

public class Schedule
{
	public int ScheduleId { get; set; }
	public int TutorId { get; set; }
	public DateOnly Date { get; set; }
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }
	public bool IsAvailable { get; set; } = true;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public virtual Tutor Tutor { get; set; } = null!;
	public virtual Booking? Booking { get; set; }
}
