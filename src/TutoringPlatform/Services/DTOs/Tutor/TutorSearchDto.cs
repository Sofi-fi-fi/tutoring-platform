namespace TutoringPlatform.Services.DTOs.Tutor;

public class TutorSearchDto
{
	public int? CityID { get; set; }
	public int? SubjectId { get; set; }
	public int? LevelId { get; set; }
	public decimal? MinPrice { get; set; }
	public decimal? MaxPrice { get; set; }
	public bool? OnlineOnly { get; set; }
	public bool? OfflineOnly { get; set; }
}