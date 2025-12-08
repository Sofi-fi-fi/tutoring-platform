using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
	Task<IEnumerable<Review>> GetByTutorIdAsync(int tutorId);
	Task<IEnumerable<Review>> GetByStudentIdAsync(int studentId);
	Task<Review?> GetByBookingIdAsync(int bookingId);
	Task<IEnumerable<Review>> GetByRatingAsync(int rating); 
}