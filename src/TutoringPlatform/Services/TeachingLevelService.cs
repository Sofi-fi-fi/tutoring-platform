using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Repositories.Interfaces;
using TutoringPlatform.Services.DTOs.TeachingLevel;

namespace TutoringPlatform.Services;

public class TeachingLevelService(
	ITeachingLevelRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<TeachingLevelService> logger) : BaseService<TeachingLevel, TeachingLevelDto, CreateTeachingLevelDto, UpdateTeachingLevelDto>(repository, context, mapper, logger), ITeachingLevelService
{
    private readonly ITeachingLevelRepository _teachingLevelRepository = repository;

    public override async Task<TeachingLevelDto> CreateAsync(CreateTeachingLevelDto createDto)
    {
        _logger.LogInformation("Creating teaching level {LevelName} at position {Position}", 
            createDto.Name, createDto.Position);

        var nameExists = await _context.TeachingLevels.AnyAsync(l => l.Name == createDto.Name);
        if (nameExists)
        {
            throw new InvalidOperationException($"Teaching level with name {createDto.Name} already exists");
        }

        var positionExists = await _context.TeachingLevels.AnyAsync(l => l.Position == createDto.Position);
        if (positionExists)
        {
            throw new InvalidOperationException($"Teaching level position {createDto.Position} is already in use");
        }

        var level = _mapper.Map<TeachingLevel>(createDto);
        await _context.TeachingLevels.AddAsync(level);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Teaching level {LevelId} created successfully", level.LevelId);

        var createdLevel = await _teachingLevelRepository.GetByIdAsync(level.LevelId);
        return _mapper.Map<TeachingLevelDto>(createdLevel);
    }

    public override async Task<TeachingLevelDto> UpdateAsync(int id, UpdateTeachingLevelDto updateDto)
    {
        var level = await _context.TeachingLevels
            .FirstOrDefaultAsync(l => l.LevelId == id)
            ?? throw new KeyNotFoundException($"Teaching level with id {id} not found");

        var nameExists = await _context.TeachingLevels
            .AnyAsync(l => l.LevelId != id && l.Name == updateDto.Name);

        if (nameExists)
        {
            throw new InvalidOperationException($"Teaching level with name {updateDto.Name} already exists");
        }

        var positionExists = await _context.TeachingLevels
            .AnyAsync(l => l.LevelId != id && l.Position == updateDto.Position);

        if (positionExists)
        {
            throw new InvalidOperationException($"Teaching level position {updateDto.Position} is already in use");
        }

        level.Name = updateDto.Name;
        level.Position = updateDto.Position;
        level.Description = updateDto.Description;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Teaching level {LevelId} updated successfully", id);
        return _mapper.Map<TeachingLevelDto>(level);
    }

    public override async Task DeleteAsync(int id)
    {
        var level = await _context.TeachingLevels
            .Include(l => l.TutorSubjects)
            .FirstOrDefaultAsync(l => l.LevelId == id)
            ?? throw new KeyNotFoundException($"Teaching level with id {id} not found");

        if (level.TutorSubjects.Any())
        {
            throw new InvalidOperationException("Cannot delete a teaching level that is assigned to tutor subjects");
        }

        _context.TeachingLevels.Remove(level);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Teaching level {LevelId} deleted successfully", id);
    }

    public async Task<TeachingLevelDto?> GetByNameAsync(string name)
    {
        var level = await _teachingLevelRepository.GetByNameAsync(name);
        return level is null ? null : _mapper.Map<TeachingLevelDto>(level);
    }

    public async Task<TeachingLevelDto?> GetByPositionAsync(int position)
    {
        var level = await _teachingLevelRepository.GetByPositionAsync(position);
        return level is null ? null : _mapper.Map<TeachingLevelDto>(level);
    }

    public async Task<IEnumerable<TeachingLevelDto>> GetOrderedByPositionAsync()
    {
        var levels = await _teachingLevelRepository.GetOrderedByPositionAsync();
        return _mapper.Map<IEnumerable<TeachingLevelDto>>(levels);
    }
}