using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class ScheduleRepository(TutoringDbContext context) : Repository<Schedule>(context), IScheduleRepository
{
	public async Task<IEnumerable<Schedule>> GetAllSlotsByTutorIdAsync(int tutorId)
	{
		return await _dbSet
		  .Where(s => s.TutorId == tutorId)
		  .ToListAsync();
	}

	public async Task<IEnumerable<Schedule>> GetAvailableSlotsByTutorIdAsync(int tutorId)
	{
		return await _dbSet
		  .Where(s => s.TutorId == tutorId)
		  .Where(s => s.IsAvailable == true)
		  .ToListAsync();
	}

	public async Task<IEnumerable<Schedule>> GetByTutorIdDateAsync(int tutorId, DateOnly date)
	{
		return await _dbSet
		  .Where(s => s.TutorId == tutorId && s.Date == date)
		  .ToListAsync();
	}
}