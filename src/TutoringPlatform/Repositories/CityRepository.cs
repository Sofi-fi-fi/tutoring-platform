using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class CityRepository(TutoringDbContext context) : Repository<City>(context), ICityRepository
{
	public async Task<City?> GetByNameAsync(string name)
	{
		return await _dbSet
			.FirstOrDefaultAsync(c => c.Name == name);
	}

	public async Task<IEnumerable<City>> GetByCountryAsync(string country)
	{
		return await _dbSet
			.Where(c => c.Country == country)
			.ToListAsync();
	}
}