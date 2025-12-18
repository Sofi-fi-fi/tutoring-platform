namespace TutoringPlatform.Services.DTOs.TutorSubject;

public class TutorPricingStatisticsDto
{
    public int TutorId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public decimal MinRate { get; set; }
    public decimal MaxRate { get; set; }
    public decimal AvgRate { get; set; }
}