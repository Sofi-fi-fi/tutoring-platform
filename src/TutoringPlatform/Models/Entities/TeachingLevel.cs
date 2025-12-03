using System;

namespace TutoringPlatform.Models.Entities;

public class TeachingLevel
{
	public int LevelId { get; set; }
	public string Name { get; set; } = null!;
	public short Position { get; set; }
	public string? Description { get; set; }

	public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}
