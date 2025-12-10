using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface ITutorSubjectRepository : IRepository<TutorSubject>
{
    Task<IEnumerable<TutorSubject>> GetByTutorIdAsync(int tutorId);
    Task<IEnumerable<TutorSubject>> GetBySubjectIdAsync(int subjectId);
    Task<TutorSubject?> GetByTutorSubjectLevelAsync(int tutorId, int subjectId, int levelId);
}