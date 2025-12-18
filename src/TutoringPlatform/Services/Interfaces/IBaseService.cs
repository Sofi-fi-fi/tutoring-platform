namespace TutoringPlatform.Services.Interfaces;

public interface IBaseService<TDto, TCreateDto, TUpdateDto>
{
    Task<TDto?> GetByIdAsync(int id);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> CreateAsync(TCreateDto createDto);
    Task<TDto> UpdateAsync(int id, TUpdateDto updateDto);
    Task DeleteAsync(int id);
}