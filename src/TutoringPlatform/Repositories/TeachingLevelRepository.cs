using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class TeachingLevelRepository(TutoringDbContext context) : Repository<TeachingLevel>(context), ITeachingLevelRepository
{
	public async Task<TeachingLevel?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(l => l.Name == name);
    }

    public async Task<TeachingLevel?> GetByPositionAsync(int position)
    {
        return await _dbSet
            .FirstOrDefaultAsync(l => l.Position == position);
    }

    public async Task<IEnumerable<TeachingLevel>> GetOrderedByPositionAsync()
    {
        return await _dbSet
            .OrderBy(l => l.Position)
            .ToListAsync();
    }
}