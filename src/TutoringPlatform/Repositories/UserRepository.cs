using TutoringPlatform.Models;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Repositories;

public class UserRepository(TutoringDbContext context) : Repository<User>(context), IUserRepository
{
	public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Student)
            .Include(u => u.Tutor)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

	public async Task<User?> GetByPhoneAsync(string phone)
	{
		return await _dbSet
			.Include(u => u.Student)
			.Include(u => u.Tutor)
			.FirstOrDefaultAsync(u => u.Phone == phone);
	}

    public async Task<IEnumerable<User>> GetByTypeAsync(UserType userType)
    {
        return await _dbSet
            .Where(u => u.UserType == userType)
            .OrderBy(u => u.RegistrationDate)
            .ToListAsync();
    }
}