using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class SubjectRepository(TutoringDbContext context) : Repository<Subject>(context), ISubjectRepository
{
	public async Task<Subject?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<IEnumerable<Subject>> GetByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(s => s.Category == category)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }
}