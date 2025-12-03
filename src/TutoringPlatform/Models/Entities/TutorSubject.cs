using System;


namespace TutoringPlatform.Models.Entities;

public class TutorSubject
{
	public int TutorSubjectId { get; set; }
	public int TutorId { get; set; }
	public int SubjectId { get; set; }
	public int LevelId { get; set; }
	public decimal HourlyRate { get; set; }

	public virtual Tutor Tutor { get; set; } = null!;
	public virtual Subject Subject { get; set; } = null!;
	public virtual TeachingLevel TeachingLevel { get; set; } = null!;
	public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
