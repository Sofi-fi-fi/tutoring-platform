using System;
using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Models.Entities;

public class Subject
{
	public int SubjectId { get; set; }
	public string Name { get; set; } = string.Empty;
	public SubjectCategory Category { get; set; }
	public string? Description { get; set; }

	public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}
