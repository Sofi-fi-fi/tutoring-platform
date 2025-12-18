using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Services.DTOs.TutorSubject;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public class TutorSubjectService(
	ITutorSubjectRepository repository,
	IMapper mapper,
	TutoringDbContext context,
	ILogger<TutorSubjectService> logger
) : BaseService<TutorSubject, TutorSubjectDto, CreateTutorSubjectDto, UpdateTutorSubjectDto>(repository, context, mapper, logger), ITutorSubjectService
{
	private readonly ITutorSubjectRepository _tutorSubjectRepository = repository;
    
    public override async Task<TutorSubjectDto> CreateAsync(CreateTutorSubjectDto createDto)
    {
        _logger.LogInformation("Creating tutor-subject entry. Tutor: {TutorId}, Subject: {SubjectId}, Level: {LevelId}", 
            createDto.TutorId, createDto.SubjectId, createDto.LevelId);

        var tutorExists = await _context.Tutors.AnyAsync(t => t.TutorId == createDto.TutorId);
        if (!tutorExists)
        {
            throw new KeyNotFoundException($"Tutor with id {createDto.TutorId} not found");
        }

        var subjectExists = await _context.Subjects.AnyAsync(s => s.SubjectId == createDto.SubjectId);
        if (!subjectExists)
        {
            throw new KeyNotFoundException($"Subject with id {createDto.SubjectId} not found");
        }

        var levelExists = await _context.TeachingLevels.AnyAsync(l => l.LevelId == createDto.LevelId);
        if (!levelExists)
        {
            throw new KeyNotFoundException($"Teaching level with id {createDto.LevelId} not found");
        }

        var duplicateExists = await _context.TutorSubjects.AnyAsync(ts =>
            ts.TutorId == createDto.TutorId &&
            ts.SubjectId == createDto.SubjectId &&
            ts.LevelId == createDto.LevelId);

        if (duplicateExists)
        {
            throw new InvalidOperationException("Tutor already offers this subject at the specified level");
        }

        var tutorSubject = _mapper.Map<TutorSubject>(createDto);
        await _context.TutorSubjects.AddAsync(tutorSubject);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Tutor-subject {TutorSubjectId} created successfully", tutorSubject.TutorSubjectId);

        var createdTutorSubject = await _tutorSubjectRepository.GetByIdAsync(tutorSubject.TutorSubjectId);
        return _mapper.Map<TutorSubjectDto>(createdTutorSubject);
    }

    public override async Task<TutorSubjectDto> UpdateAsync(int id, UpdateTutorSubjectDto updateDto)
    {
        var tutorSubject = await _context.TutorSubjects
            .Include(ts => ts.Bookings)
            .FirstOrDefaultAsync(ts => ts.TutorSubjectId == id)
            ?? throw new KeyNotFoundException($"Tutor-subject with id {id} not found");

        var hasActiveBookings = tutorSubject.Bookings.Any(b => 
            b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed);

        if (hasActiveBookings)
        {
            throw new InvalidOperationException("Cannot update hourly rate while there are active bookings");
        }

        tutorSubject.HourlyRate = updateDto.HourlyRate;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Tutor-subject {TutorSubjectId} updated successfully", id);
        return _mapper.Map<TutorSubjectDto>(tutorSubject);
    }

    public override async Task DeleteAsync(int id)
    {
        var tutorSubject = await _context.TutorSubjects
            .Include(ts => ts.Bookings)
            .FirstOrDefaultAsync(ts => ts.TutorSubjectId == id)
            ?? throw new KeyNotFoundException($"Tutor-subject with id {id} not found");

        if (tutorSubject.Bookings.Any())
        {
            throw new InvalidOperationException("Cannot delete tutor-subject entries that have bookings");
        }

        _context.TutorSubjects.Remove(tutorSubject);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Tutor-subject {TutorSubjectId} deleted successfully", id);
    }

    public async Task<IEnumerable<TutorSubjectDto>> GetByTutorIdAsync(int tutorId)
    {
        var tutorSubjects = await _tutorSubjectRepository.GetByTutorIdAsync(tutorId);
        return _mapper.Map<IEnumerable<TutorSubjectDto>>(tutorSubjects);
    }

    public async Task<IEnumerable<TutorSubjectDto>> GetBySubjectIdAsync(int subjectId)
    {
        var tutorSubjects = await _tutorSubjectRepository.GetBySubjectIdAsync(subjectId);
        return _mapper.Map<IEnumerable<TutorSubjectDto>>(tutorSubjects);
    }

    public async Task<IEnumerable<TutorSubjectDto>> GetByLevelIdAsync(int levelId)
    {
        var tutorSubjects = await _tutorSubjectRepository.GetByLevelIdAsync(levelId);
        return _mapper.Map<IEnumerable<TutorSubjectDto>>(tutorSubjects);
    }
    
    public async Task<IEnumerable<TutorPricingStatisticsDto>> GetTutorPricingStatisticsAsync()
    {        
        var result = await _tutorSubjectRepository.GetTutorPricingStatisticsAsync();
        return _mapper.Map<IEnumerable<TutorPricingStatisticsDto>>(result);
    }
}
