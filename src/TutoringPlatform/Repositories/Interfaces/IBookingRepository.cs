using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
	Task<IEnumerable<Booking>> GetByStudentIdAsync(int studentId);
	Task<IEnumerable<Booking>> GetByTutorIdAsync(int tutorId);
	Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status);
}