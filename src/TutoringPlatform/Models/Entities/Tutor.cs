using System;

namespace TutoringPlatform.Models.Entities;

public class Tutor
{
	public int TutorId { get; set; }
	public int? CityId { get; set; }
	public short YearsExperience { get; set; } = 0;
	public string Education { get; set; } = null!;
	public string? AboutMe { get; set; }
	public bool OnlineAvailable { get; set; } = true;
	public bool OfflineAvailable { get; set; } = true;
	public string? Address { get; set; }

	public virtual User User { get; set; } = null!;
	public virtual City? City { get; set; }
	public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
	public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
