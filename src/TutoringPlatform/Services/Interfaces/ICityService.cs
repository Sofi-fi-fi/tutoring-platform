using TutoringPlatform.Services.DTOs.City;

namespace TutoringPlatform.Services.Interfaces;

public interface ICityService : IBaseService<CityDto, CreateCityDto, UpdateCityDto>
{
    Task<CityDto?> GetByNameAsync(string name);
    Task<IEnumerable<CityDto>> GetByCountryAsync(string country);
}