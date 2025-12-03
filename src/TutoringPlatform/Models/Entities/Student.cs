using System;

namespace TutoringPlatform.Models.Entities;

public class Student
{
	public int StudentId { get; set; }
	public int? CityId { get; set; }
	public short? SchoolGrade { get; set; }

	public virtual User User { get; set; } = null!;
	public virtual City? City { get; set; }
	public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
