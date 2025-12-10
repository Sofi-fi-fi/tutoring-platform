using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class TutorSubjectRepository(TutoringDbContext context) : Repository<TutorSubject>(context), ITutorSubjectRepository
{
	public async Task<IEnumerable<TutorSubject>> GetByTutorIdAsync(int tutorId)
    {
        return await _dbSet
            .Include(ts => ts.Subject)
            .Include(ts => ts.TeachingLevel)
            .Where(ts => ts.TutorId == tutorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TutorSubject>> GetBySubjectIdAsync(int subjectId)
    {
        return await _dbSet
            .Include(ts => ts.Tutor)
                .ThenInclude(t => t.User)
            .Include(ts => ts.TeachingLevel)
            .Where(ts => ts.SubjectId == subjectId)
            .ToListAsync();
    }

    public async Task<TutorSubject?> GetByTutorSubjectLevelAsync(int tutorId, int subjectId, int levelId)
    {
        return await _dbSet
            .Include(ts => ts.Tutor)
            .Include(ts => ts.Subject)
            .Include(ts => ts.TeachingLevel)
            .FirstOrDefaultAsync(ts => ts.TutorId == tutorId && 
                ts.SubjectId == subjectId && 
                ts.LevelId == levelId);
    }
}