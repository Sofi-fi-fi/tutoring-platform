using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface ITeachingLevelRepository : IRepository<TeachingLevel>
{
    Task<TeachingLevel?> GetByNameAsync(string name);
    Task<IEnumerable<TeachingLevel>> GetOrderedByPositionAsync();
}