using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Services.DTOs.Booking;

public class UpdateBookingDto
{
	public BookingStatus Status { get; set; }
}