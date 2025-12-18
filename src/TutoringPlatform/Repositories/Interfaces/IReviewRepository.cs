using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
	Task<Review?> GetByBookingAsync(int bookingId);
	Task<IEnumerable<Review>> GetByStudentIdAsync(int studentId);
	Task<IEnumerable<Review>> GetByTutorIdAsync(int tutorId);
	Task<IEnumerable<Review>> GetByRatingAsync(int rating); 
}