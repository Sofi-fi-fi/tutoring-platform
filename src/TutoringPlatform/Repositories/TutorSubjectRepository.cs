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

    public async Task<IEnumerable<TutorSubject>> GetByLevelIdAsync(int levelId)
    {
        return await _dbSet
            .Include(ts => ts.Tutor)
                .ThenInclude(t => t.User)
            .Include(ts => ts.Subject)
            .Where(ts => ts.LevelId == levelId)
            .ToListAsync();
    }

    public async Task<IEnumerable<(int TutorId, string TutorName, decimal MinRate, decimal MaxRate, decimal AvgRate)>> GetTutorPricingStatisticsAsync()
    {
        var rows = await _dbSet
            .AsNoTracking()
            .GroupBy(ts => new
            {
                ts.TutorId,
                ts.Tutor.User.FirstName,
                ts.Tutor.User.LastName
            })
            .Select(g => new
            {
                g.Key.TutorId,
                TutorName = g.Key.FirstName + " " + g.Key.LastName,
                MinRate = g.Min(x => x.HourlyRate),
                MaxRate = g.Max(x => x.HourlyRate),
                AvgRate = g.Average(x => x.HourlyRate)
            })
            .OrderBy(x => x.TutorId)
            .ToListAsync();

        return [.. rows.Select(x => (x.TutorId, x.TutorName, x.MinRate, x.MaxRate, x.AvgRate))];
    }
}