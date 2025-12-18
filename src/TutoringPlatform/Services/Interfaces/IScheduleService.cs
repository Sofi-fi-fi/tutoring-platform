using TutoringPlatform.Services.DTOs.Schedule;

namespace TutoringPlatform.Services.Interfaces;

public interface IScheduleService : IBaseService<ScheduleDto, CreateScheduleDto, UpdateScheduleDto>
{
    Task<IEnumerable<ScheduleDto>> GetAllSlotsByTutorIdAsync(int tutorId);
    Task<IEnumerable<ScheduleDto>> GetAvailableSlotsByTutorAsync(int tutorId);
    Task<IEnumerable<ScheduleDto>> GetByTutorIdDateAsync(int tutorId, DateOnly date);
    Task<ScheduleDto> MarkAsUnavailableAsync(int scheduleId);
    Task<ScheduleDto> MarkAsAvailableAsync(int scheduleId);
}