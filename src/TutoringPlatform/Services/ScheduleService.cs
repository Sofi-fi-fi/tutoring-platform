using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Services.DTOs.Schedule;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public class ScheduleService(
	IScheduleRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<ScheduleService> logger
) : BaseService<Schedule, ScheduleDto, CreateScheduleDto, UpdateScheduleDto>(repository, context, mapper, logger), IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository = repository;

    public override async Task<ScheduleDto> CreateAsync(CreateScheduleDto createDto)
    {
        _logger.LogInformation("Creating schedule for tutor {TutorId} on {Date} from {Start} to {End}", 
            createDto.TutorId, createDto.Date, createDto.StartTime, createDto.EndTime);

        var tutorExists = await _context.Tutors.AnyAsync(t => t.TutorId == createDto.TutorId);
        if (!tutorExists)
        {
            throw new KeyNotFoundException($"Tutor with id {createDto.TutorId} not found");
        }

        if (createDto.Date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Cannot create schedules in the past");
        }

        var overlaps = await _context.Schedules.AnyAsync(s =>
            s.TutorId == createDto.TutorId &&
            s.Date == createDto.Date &&
            s.StartTime < createDto.EndTime &&
            s.EndTime > createDto.StartTime);

        if (overlaps)
        {
            throw new InvalidOperationException("Schedule overlaps with an existing slot for this tutor");
        }

        var schedule = _mapper.Map<Schedule>(createDto);
        await _context.Schedules.AddAsync(schedule);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Schedule {ScheduleId} created successfully", schedule.ScheduleId);

        var createdSchedule = await _scheduleRepository.GetByIdAsync(schedule.ScheduleId);
        return _mapper.Map<ScheduleDto>(createdSchedule);
    }

    public override async Task<ScheduleDto> UpdateAsync(int id, UpdateScheduleDto updateDto)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Booking)
            .FirstOrDefaultAsync(s => s.ScheduleId == id)
            ?? throw new KeyNotFoundException($"Schedule with id {id} not found");

        if (schedule.Booking is not null)
        {
            throw new InvalidOperationException("Cannot update a schedule that already has a booking");
        }

        if (updateDto.Date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Cannot move schedule to a past date");
        }

        var overlaps = await _context.Schedules.AnyAsync(s =>
            s.ScheduleId != id &&
            s.TutorId == schedule.TutorId &&
            s.Date == updateDto.Date &&
            s.StartTime < updateDto.EndTime &&
            s.EndTime > updateDto.StartTime);

        if (overlaps)
        {
            throw new InvalidOperationException("Updated time slot overlaps with an existing schedule for this tutor");
        }

        schedule.Date = updateDto.Date;
        schedule.StartTime = updateDto.StartTime;
        schedule.EndTime = updateDto.EndTime;
        schedule.IsAvailable = updateDto.IsAvailable;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Schedule {ScheduleId} updated successfully", id);
        return _mapper.Map<ScheduleDto>(schedule);
    }

    public override async Task DeleteAsync(int id)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Booking)
            .FirstOrDefaultAsync(s => s.ScheduleId == id)
            ?? throw new KeyNotFoundException($"Schedule with id {id} not found");

        if (schedule.Booking is not null)
        {
            throw new InvalidOperationException("Cannot delete a schedule that has an associated booking");
        }

        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Schedule {ScheduleId} deleted successfully", id);
    }

    public async Task<IEnumerable<ScheduleDto>> GetAllSlotsByTutorIdAsync(int tutorId)
    {
        var schedules = await _scheduleRepository.GetAllSlotsByTutorIdAsync(tutorId);
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<IEnumerable<ScheduleDto>> GetAvailableSlotsByTutorAsync(int tutorId)
    {
        var schedules = await _scheduleRepository.GetAvailableSlotsByTutorIdAsync(tutorId);
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<IEnumerable<ScheduleDto>> GetByTutorIdDateAsync(int tutorId, DateOnly date)
    {
        var schedules = await _scheduleRepository.GetByTutorIdDateAsync(tutorId, date);
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<ScheduleDto> MarkAsUnavailableAsync(int scheduleId)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Booking)
            .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId) ?? throw new KeyNotFoundException($"Schedule with id {scheduleId} not found");

        if (schedule.Booking is not null)
        {
            throw new InvalidOperationException("Cannot mark schedule as unavailable - it already has a booking");
        }

        if (!schedule.IsAvailable)
        {
            throw new InvalidOperationException("Schedule is already marked as unavailable");
        } 

        schedule.IsAvailable = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Schedule {ScheduleId} marked as unavailable", scheduleId);
        return _mapper.Map<ScheduleDto>(schedule);
    }

    public async Task<ScheduleDto> MarkAsAvailableAsync(int scheduleId)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Booking)
            .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId) ?? throw new KeyNotFoundException($"Schedule with id {scheduleId} not found");

        if (schedule.Booking is not null)
        {
            throw new InvalidOperationException("Cannot mark schedule as available - it has an associated booking");
        }

        if (schedule.IsAvailable)
        {
            throw new InvalidOperationException("Schedule is already marked as available");
        }

        if (schedule.Date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Cannot mark past schedule as available");
        }

        schedule.IsAvailable = true;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Schedule {ScheduleId} marked as available", scheduleId);
        return _mapper.Map<ScheduleDto>(schedule);
    }
}