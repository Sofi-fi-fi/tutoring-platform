using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
	Task<IEnumerable<Student>> GetByCityIdAsync(int cityId);
}