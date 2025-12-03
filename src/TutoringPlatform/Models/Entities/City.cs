using System;


namespace TutoringPlatform.Models.Entities;

public class City
{
	public int CityId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Region { get; set; }
	public string Country { get; set; } = "Україна";

	public virtual ICollection<Student> Students { get; set; } = new List<Student>();
	public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
