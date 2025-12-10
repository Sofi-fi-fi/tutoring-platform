using TutoringPlatform.Models.Enums;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Repositories.Interfaces;

public interface ISubjectRepository : IRepository<Subject>
{
    Task<Subject?> GetByNameAsync(string name);
    Task<IEnumerable<Subject>> GetByCategoryAsync(SubjectCategory category);
}