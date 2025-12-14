namespace TutoringPlatform.Services.DTOs.City;

public class UpdateCityDto
{
	public string Name { get; set; } = string.Empty;
	public string? Region { get; set; }
	public string Country { get; set; } = string.Empty;
}