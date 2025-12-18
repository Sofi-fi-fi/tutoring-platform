using TutoringPlatform.Services.DTOs.Student;

namespace TutoringPlatform.Services.Interfaces;

public interface IStudentService : IBaseService<StudentDto, CreateStudentDto, UpdateStudentDto>
{
    Task<IEnumerable<StudentDto>> GetByCityAsync(int cityId);
    Task<(IEnumerable<StudentDto> Students, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, int? cityId = null, short? schoolGrade = null);
}