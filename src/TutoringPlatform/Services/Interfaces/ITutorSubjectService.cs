using TutoringPlatform.Services.DTOs.TutorSubject;

namespace TutoringPlatform.Services.Interfaces;

public interface ITutorSubjectService : IBaseService<TutorSubjectDto, CreateTutorSubjectDto, UpdateTutorSubjectDto>
{
    Task<IEnumerable<TutorSubjectDto>> GetByTutorIdAsync(int tutorId);
    Task<IEnumerable<TutorSubjectDto>> GetBySubjectIdAsync(int subjectId);
    Task<IEnumerable<TutorSubjectDto>> GetByLevelIdAsync(int levelId);
    Task<IEnumerable<TutorPricingStatisticsDto>> GetTutorPricingStatisticsAsync();
}