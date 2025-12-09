using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class ReviewRepository(TutoringDbContext context) : Repository<Review>(context), IReviewRepository
{
	public async Task<IEnumerable<Review>> GetByTutorIdAsync(int tutorId)
	{
		return await _dbSet
			.Include(r => r.Booking)
				.ThenInclude(b => b.Schedule)
			.Where(r => r.Booking.Schedule.TutorId == tutorId)
			.ToListAsync();
	}

	public async Task<IEnumerable<Review>> GetByStudentIdAsync(int studentId)
	{
		return await _dbSet
			.Include(r => r.Booking)
			.Where(r => r.Booking.StudentId == studentId)
			.ToListAsync();
	}

	public async Task<Review?> GetByBookingIdAsync(int bookingId)
	{
		return await _dbSet 
			.Where(r => r.BookingId == bookingId)
			.FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<Review>> GetByRatingAsync(int rating)
	{
		return await _dbSet
			.Where(r => r.Rating == rating)
			.ToListAsync();
	}
}