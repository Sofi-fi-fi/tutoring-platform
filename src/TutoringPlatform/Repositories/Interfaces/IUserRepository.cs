using TutoringPlatform.Models.Enums;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
	Task<User?> GetByEmailAsync(string email);
	Task<User?> GetByPhoneAsync(string phone);
    Task<IEnumerable<User>> GetByTypeAsync(UserType userType);
}