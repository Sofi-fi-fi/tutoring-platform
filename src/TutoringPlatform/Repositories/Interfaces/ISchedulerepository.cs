using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IScheduleRepository : IRepository<Schedule>
{
	Task<IEnumerable<Schedule>> GetByTutorIdAsync(int tutorId);
	Task<IEnumerable<Schedule>> GetAvailableSlotsByTutorIdAsync(int tutorId);
	Task<IEnumerable<Schedule>> GetByTutorIdDateAsync(int tutorId, DateOnly date);
	Task<Schedule?> GetByTutorIdDateTimeAsync(int tutorId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
}