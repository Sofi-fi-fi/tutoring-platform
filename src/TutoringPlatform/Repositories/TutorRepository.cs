using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class TutorRepository(TutoringDbContext context) : Repository<Tutor>(context), ITutorRepository
{
    public override async Task<Tutor?> GetByIdAsync(int tutorId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => t.TutorId == tutorId)
            .FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<Tutor>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.User)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Tutor>> GetByCityAsync(int cityId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => t.CityId == cityId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tutor>> SearchTutorsAsync(int? cityId, int? subjectId, int? levelId, decimal? minPrice, decimal? maxPrice, bool? onlineOnly, bool? offlineOnly)
	{
        var query = _dbSet
            .Include(t => t.User)
            .Include(t => t.City)
            .Include(t => t.TutorSubjects)
                .ThenInclude(ts => ts.Subject)
            .Include(t => t.TutorSubjects)
                .ThenInclude(ts => ts.TeachingLevel)
            .AsQueryable();

        if (cityId.HasValue)
        {
            query = query.Where(t => t.CityId == cityId.Value);
        }

        if (onlineOnly.HasValue)
        {
            query = query.Where(t => t.OnlineAvailable == onlineOnly.Value);
        }

        if (offlineOnly.HasValue)
        {
            query = query.Where(t => t.OfflineAvailable == offlineOnly.Value);
        }

        if (subjectId.HasValue)
        {
            query = query.Where(t => t.TutorSubjects.Any(ts => ts.SubjectId == subjectId.Value));
        }

        if (levelId.HasValue)
        {
            query = query.Where(t => t.TutorSubjects.Any(ts => ts.LevelId == levelId.Value));
        }

        if (minPrice.HasValue || maxPrice.HasValue)
        {
            query = query.Where(t => t.TutorSubjects.Any(ts =>
                (!minPrice.HasValue || ts.HourlyRate >= minPrice.Value) &&
                (!maxPrice.HasValue || ts.HourlyRate <= maxPrice.Value)));
        }

        return await query.ToListAsync();
	}

    public async Task<IEnumerable<(Tutor Tutor, double AverageRating, int ReviewCount)>> GetTopRatedAsync(int count)
    {
        var results = await _dbSet
            .Include(t => t.User)
            .Include(t => t.City)
            .Include(t => t.TutorSubjects)
                .ThenInclude(ts => ts.Subject)
            .Include(t => t.TutorSubjects)
                .ThenInclude(ts => ts.TeachingLevel)
            .Include(t => t.Schedules)
                .ThenInclude(sc => sc.Booking)
                .ThenInclude(b => b!.Review)
            .Select(t => new
            {
                Tutor = t,
                AverageRating = t.Schedules
                    .Where(s => s.Booking != null && s.Booking.Review != null)
                    .Average(s => (double?)s.Booking!.Review!.Rating) ?? 0,
                ReviewCount = t.Schedules
                    .Count(s => s.Booking != null && s.Booking.Review != null)
            })
            .Where(x => x.ReviewCount > 0)
            .OrderByDescending(x => x.AverageRating)
            .ThenByDescending(x => x.ReviewCount)
            .Take(count)
            .ToListAsync();

        return results.Select(x => (x.Tutor, x.AverageRating, x.ReviewCount));
    }
}