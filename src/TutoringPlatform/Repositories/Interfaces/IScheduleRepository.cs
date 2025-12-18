using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IScheduleRepository : IRepository<Schedule>
{
	Task<IEnumerable<Schedule>> GetAllSlotsByTutorIdAsync(int tutorId);
	Task<IEnumerable<Schedule>> GetAvailableSlotsByTutorIdAsync(int tutorId);
	Task<IEnumerable<Schedule>> GetByTutorIdDateAsync(int tutorId, DateOnly date);
}