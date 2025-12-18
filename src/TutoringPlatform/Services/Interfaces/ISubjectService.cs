using TutoringPlatform.Services.DTOs.Subject;

namespace TutoringPlatform.Services.Interfaces;

public interface ISubjectService : IBaseService<SubjectDto, CreateSubjectDto, UpdateSubjectDto>
{
    Task<SubjectDto?> GetByNameAsync(string name);
    Task<IEnumerable<SubjectDto>> GetByCategoryAsync(string category);
}