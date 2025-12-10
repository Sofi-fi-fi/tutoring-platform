using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class TutorRepository(TutoringDbContext context) : Repository<Tutor>(context), ITutorRepository
{
	public async Task<Tutor?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.City)
            .Include(t => t.TutorSubjects)
                .ThenInclude(ts => ts.Subject)
            .Include(t => t.TutorSubjects)
                .ThenInclude(ts => ts.TeachingLevel)
            .Include(t => t.Schedules)
				.ThenInclude(sc => sc.Booking)
				.ThenInclude(b => b!.Review)
            .FirstOrDefaultAsync(t => t.TutorId == id);
    }

    public async Task<IEnumerable<Tutor>> GetByCityAsync(int cityId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.City)
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

        if (onlineOnly == true)
        {
            query = query.Where(t => t.OnlineAvailable);
        }

        if (offlineOnly == true)
        {
            query = query.Where(t => t.OfflineAvailable);
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

    public async Task<IEnumerable<Tutor>> GetTopRatedAsync(int count)
    {
        return await _dbSet
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
                    .Average(s => (double?)s.Booking!.Review!.Rating)
            })
            .Where(x => x.AverageRating != null)
            .OrderByDescending(x => x.AverageRating)
            .Take(count)
            .Select(x => x.Tutor)
            .ToListAsync();
    }
}