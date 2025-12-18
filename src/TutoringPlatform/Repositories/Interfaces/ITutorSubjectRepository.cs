using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface ITutorSubjectRepository : IRepository<TutorSubject>
{
    Task<IEnumerable<TutorSubject>> GetByTutorIdAsync(int tutorId);
    Task<IEnumerable<TutorSubject>> GetBySubjectIdAsync(int subjectId);
    Task<IEnumerable<TutorSubject>> GetByLevelIdAsync(int levelId);
    Task<IEnumerable<(int TutorId, string TutorName, decimal MinRate, decimal MaxRate, decimal AvgRate)>> GetTutorPricingStatisticsAsync();
}