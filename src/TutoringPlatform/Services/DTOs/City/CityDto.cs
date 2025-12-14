namespace TutoringPlatform.Services.DTOs.City;

public class CityDto
{
    public int CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Region { get; set; }
    public string Country { get; set; } = string.Empty;
}