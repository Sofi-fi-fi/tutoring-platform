using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface ITutorRepository : IRepository<Tutor>
{
    Task<IEnumerable<Tutor>> GetByCityAsync(int cityId);
    Task<IEnumerable<Tutor>> SearchTutorsAsync(int? cityId, int? subjectId, int? levelId, decimal? minPrice, decimal? maxPrice, bool? onlineOnly, bool? offlineOnly);
    Task<IEnumerable<(Tutor Tutor, double AverageRating, int ReviewCount)>> GetTopRatedAsync(int count);
}