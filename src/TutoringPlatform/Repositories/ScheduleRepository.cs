using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class ScheduleRepository(TutoringDbContext context) : Repository<Schedule>(context), IScheduleRepository
{
	public async Task<IEnumerable<Schedule>> GetByTutorIdAsync(int tutorId)
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

	public async Task<Schedule?> GetByTutorIdDateTimeAsync(int tutorId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
	{
		return await _dbSet
			.Where(s => s.TutorId == tutorId)
			.Where(s => s.Date == date)
			.Where(s => s.StartTime == startTime && s.EndTime == endTime)
			.FirstOrDefaultAsync();
	}
}