using TutoringPlatform.Services.DTOs.Tutor;

namespace TutoringPlatform.Services.Interfaces;

public interface ITutorService : IBaseService<TutorDto, CreateTutorDto, UpdateTutorDto>
{    
    Task<IEnumerable<TutorDto>> GetByCityAsync(int cityId);
    Task<IEnumerable<TutorDto>> SearchTutorsAsync(TutorSearchDto searchDto);    
    Task<(IEnumerable<TutorDto> Tutors, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, string? sortBy = null, bool descending = false);
    Task<IEnumerable<TutorTopRatedDto>> GetTopRatedAsync(int count);
}