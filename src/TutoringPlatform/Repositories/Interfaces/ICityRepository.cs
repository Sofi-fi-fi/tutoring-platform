using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface ICityRepository : IRepository<City>
{
	Task<City?> GetByNameAsync(string name);
	Task<IEnumerable<City>> GetByCountryAsync(string country);
} 