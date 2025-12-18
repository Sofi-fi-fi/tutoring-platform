using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface ITeachingLevelRepository : IRepository<TeachingLevel>
{
    Task<TeachingLevel?> GetByNameAsync(string name);
    Task<TeachingLevel?> GetByPositionAsync(int position);
    Task<IEnumerable<TeachingLevel>> GetOrderedByPositionAsync();
}