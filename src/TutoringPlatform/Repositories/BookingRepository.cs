using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Repositories;

public class BookingRepository(TutoringDbContext context) : Repository<Booking>(context), IBookingRepository
{
	public async Task<IEnumerable<Booking>> GetByStudentIdAsync(int studentId)
	{
		return await _dbSet
			.Where(b => b.StudentId == studentId)
			.ToListAsync();
	}

	public async Task<IEnumerable<Booking>> GetByTutorIdAsync(int tutorId)
	{
		return await _dbSet
			.Include(b => b.TutorSubject)
			.Where(b => b.TutorSubject.TutorId == tutorId)
			.ToListAsync();
	}

	public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status)
	{
		return await _dbSet
			.Where(b => b.Status == status)
			.ToListAsync();
	}

	public async Task<Booking?> GetByIdWithDetailsAsync(int bookingId)
	{
		return await _dbSet
			.Include(b => b.Student)
				.ThenInclude(s => s.User)
			.Include(b => b.TutorSubject)
				.ThenInclude(ts => ts.Tutor)
					.ThenInclude(t => t.User)
			.Include(b => b.TutorSubject)
				.ThenInclude(ts => ts.Subject)
			.Include(b => b.Schedule)
			.Include(b => b.Review)
			.FirstOrDefaultAsync(b => b.BookingId == bookingId);
	}
}