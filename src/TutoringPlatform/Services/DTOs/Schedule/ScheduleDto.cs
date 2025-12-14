namespace TutoringPlatform.Services.DTOs.Schedule;

public class ScheduleDto
{
	public int ScheduleId { get; set; }
	public int TutorId { get; set; }
	public DateOnly Date { get; set; }
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }
	public bool IsAvailable { get; set; }
	public DateTime CreatedAt { get; set; }
}