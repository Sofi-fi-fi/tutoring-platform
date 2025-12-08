using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
	Task<Student?> GetByIdWithDetailsAsync(int studentId);
	Task<IEnumerable<Student>> GetByCityIdAsync(int cityId);
}