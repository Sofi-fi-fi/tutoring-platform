using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class StudentRepository(TutoringDbContext context) : Repository<Student>(context), IStudentRepository
{
	public async Task<Student?> GetByIdWithDetailsAsync(int studentId)
	{
		return await _dbSet
			.Include(s => s.User)
			.Include(s => s.City)
			.Include (s => s.Bookings)
			.Where(s => s.StudentId == studentId)
			.FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<Student>> GetByCityIdAsync(int cityId)
	{
		return await _dbSet
			.Where(s => s.CityId == cityId)
			.ToListAsync();
	}
}