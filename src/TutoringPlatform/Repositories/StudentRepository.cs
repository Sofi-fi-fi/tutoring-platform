using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class StudentRepository(TutoringDbContext context) : Repository<Student>(context), IStudentRepository
{
	public override async Task<Student?> GetByIdAsync(int studentId)
	{
		return await _dbSet
		  .Include(s => s.User)
		  .Where(s => s.StudentId == studentId)
		  .FirstOrDefaultAsync();
	}

	public override async Task<IEnumerable<Student>> GetAllAsync()
	{
		return await _dbSet
		  .Include(s => s.User)
		  .ToListAsync();
	}

	public async Task<IEnumerable<Student>> GetByCityIdAsync(int cityId)
	{
		return await _dbSet
		  .Include(s => s.User)
		  .Where(s => s.CityId == cityId)
		  .ToListAsync();
	}
}