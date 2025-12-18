using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.DTOs.City;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public class CityService(
	ICityRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<CityService> logger
) : BaseService<City, CityDto, CreateCityDto, UpdateCityDto>(repository, context, mapper, logger), ICityService
{
	private readonly ICityRepository _cityRepository = repository;

    public override async Task<CityDto> CreateAsync(CreateCityDto createDto)
    {
        _logger.LogInformation("Creating city {City}, {Region}, {Country}", 
            createDto.Name, createDto.Region, createDto.Country);

        var exists = await _context.Cities.AnyAsync(c => 
            c.Name == createDto.Name &&
            c.Region == createDto.Region &&
            c.Country == createDto.Country);

        if (exists)
        {
            throw new InvalidOperationException("City with the same name, region and country already exists");
        }

        var city = _mapper.Map<City>(createDto);
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        _logger.LogInformation("City {CityId} created successfully", city.CityId);

        var createdCity = await _cityRepository.GetByIdAsync(city.CityId);
        return _mapper.Map<CityDto>(city);
    }

    public override async Task<CityDto> UpdateAsync(int id, UpdateCityDto updateDto)
    {
        var city = await _context.Cities
            .FirstOrDefaultAsync(c => c.CityId == id)
            ?? throw new KeyNotFoundException($"City with id {id} not found");

        var duplicateExists = await _context.Cities.AnyAsync(c =>
            c.CityId != id &&
            c.Name == updateDto.Name &&
            c.Region == updateDto.Region &&
            c.Country == updateDto.Country);

        if (duplicateExists)
        {
            throw new InvalidOperationException("City with the same name, region and country already exists");
        }

        city.Name = updateDto.Name;
        city.Region = updateDto.Region;
        city.Country = updateDto.Country;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("City {CityId} updated successfully", id);
        return _mapper.Map<CityDto>(city);
    }

    public override async Task DeleteAsync(int id)
    {
        var city = await _context.Cities
            .Include(c => c.Students)
            .Include(c => c.Tutors)
            .FirstOrDefaultAsync(c => c.CityId == id)
            ?? throw new KeyNotFoundException($"City with id {id} not found");

        if (city.Students.Count is not 0 || city.Tutors.Count is not 0)
        {
            throw new InvalidOperationException("Cannot delete a city that has associated students or tutors");
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        _logger.LogInformation("City {CityId} deleted successfully", id);
    }

    public async Task<CityDto?> GetByNameAsync(string name)
    {
        var city = await _cityRepository.GetByNameAsync(name);
        return city is null ? null : _mapper.Map<CityDto>(city);
    }

    public async Task<IEnumerable<CityDto>> GetByCountryAsync(string country)
    {
        var cities = await _cityRepository.GetByCountryAsync(country);
        return _mapper.Map<IEnumerable<CityDto>>(cities);
    }
}