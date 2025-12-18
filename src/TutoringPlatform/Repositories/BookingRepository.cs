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
}