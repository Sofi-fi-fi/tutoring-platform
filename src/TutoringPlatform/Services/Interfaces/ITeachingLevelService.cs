using TutoringPlatform.Services.DTOs.TeachingLevel;

namespace TutoringPlatform.Services.Interfaces;

public interface ITeachingLevelService : IBaseService<TeachingLevelDto, CreateTeachingLevelDto, UpdateTeachingLevelDto>
{
    Task<TeachingLevelDto?> GetByNameAsync(string name);
    Task<TeachingLevelDto?> GetByPositionAsync(int position);
    Task<IEnumerable<TeachingLevelDto>> GetOrderedByPositionAsync();
}