using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Services.DTOs.Booking;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public class BookingService(
	IBookingRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<BookingService> logger,
	IStudentRepository studentRepository
) : BaseService<Booking, BookingDto, CreateBookingDto, UpdateBookingDto>(repository, context, mapper, logger), IBookingService
{
    private readonly IBookingRepository _bookingRepository = repository;
    private readonly IStudentRepository _studentRepository = studentRepository;

    public override async Task<BookingDto> CreateAsync(CreateBookingDto createDto)
    {
        _logger.LogInformation("Creating booking for student {StudentId}, schedule {ScheduleId}", 
            createDto.StudentId, createDto.ScheduleId);

        var student = await _studentRepository.GetByIdAsync(createDto.StudentId) ?? throw new InvalidOperationException($"Student with id {createDto.StudentId} does not exist");

        var tutorSubject = await _context.TutorSubjects
            .Include(ts => ts.Tutor)
            .FirstOrDefaultAsync(ts => ts.TutorSubjectId == createDto.TutorSubjectId) ?? throw new InvalidOperationException($"Tutor subject with id {createDto.TutorSubjectId} does not exist");

        var schedule = await _context.Schedules
            .Include(s => s.Tutor)
            .Include(s => s.Booking)
            .FirstOrDefaultAsync(s => s.ScheduleId == createDto.ScheduleId) ?? throw new InvalidOperationException($"Schedule with id {createDto.ScheduleId} does not exist");
        
        if (!schedule.IsAvailable)
        {
            throw new InvalidOperationException("Schedule slot is not available");
        }

        if (schedule.Booking is not null)
        {
            throw new InvalidOperationException("Schedule slot is already booked");
        }

        if (schedule.TutorId != tutorSubject.TutorId)
        {
            throw new InvalidOperationException("Schedule does not belong to the tutor offering this subject");
        }

        if (schedule.Date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Cannot book past schedule slots");
        }

        if (createDto.Format == BookingFormat.Online && !tutorSubject.Tutor.OnlineAvailable)
        {
            throw new InvalidOperationException("This tutor is not available for online sessions");
        }

        if (createDto.Format == BookingFormat.Offline && !tutorSubject.Tutor.OfflineAvailable)
        {
            throw new InvalidOperationException("This tutor is not available for offline sessions");
        }

        var conflictingBooking = await _context.Bookings
            .Include(b => b.Schedule)
            .Where(b => b.StudentId == createDto.StudentId)
            .Where(b => b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed)
            .Where(b => b.Schedule.Date == schedule.Date)
            .Where(b => b.Schedule.StartTime < schedule.EndTime && b.Schedule.EndTime > schedule.StartTime)
            .FirstOrDefaultAsync();

        if (conflictingBooking is not null)
        {
            throw new InvalidOperationException("Student already has a booking at this time");
        }

        schedule.IsAvailable = false;

        var booking = new Booking
        {
            StudentId = createDto.StudentId,
            TutorSubjectId = createDto.TutorSubjectId,
            ScheduleId = createDto.ScheduleId,
            Format = createDto.Format,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Booking created successfully. BookingId: {BookingId}", booking.BookingId);

        var createdBooking = await _bookingRepository.GetByIdAsync(booking.BookingId);
        return _mapper.Map<BookingDto>(createdBooking);
    }

    public override async Task<BookingDto> UpdateAsync(int bookingId, UpdateBookingDto updateDto)
    {
        var booking = await _context.Bookings
            .Include(b => b.Schedule)
            .Include(b => b.Review)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId) ?? throw new KeyNotFoundException($"Booking with id {bookingId} not found");
        
        var oldStatus = booking.Status;

        _logger.LogInformation("Updating booking {BookingId} status from {OldStatus} to {NewStatus}", 
            bookingId, oldStatus, updateDto.Status);

        var validTransition = (oldStatus, updateDto.Status) switch
        {
            (BookingStatus.Pending, BookingStatus.Confirmed) => true,
            (BookingStatus.Pending, BookingStatus.Cancelled) => true,
            (BookingStatus.Confirmed, BookingStatus.Completed) => true,
            (BookingStatus.Confirmed, BookingStatus.Cancelled) => true,
            _ => false
        };

        if (!validTransition)
        {
            throw new InvalidOperationException($"Invalid status transition from {oldStatus} to {updateDto.Status}");
        }

        if (updateDto.Status == BookingStatus.Completed && 
            booking.Schedule.Date > DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Cannot mark future bookings as completed");
        }

        if (updateDto.Status == BookingStatus.Cancelled && booking.Review is not null)
        {
            throw new InvalidOperationException("Cannot cancel a booking that has been reviewed");
        }

        booking.Status = updateDto.Status;
        
        if (updateDto.Status == BookingStatus.Cancelled)
        {
            booking.Schedule.IsAvailable = true;
            _logger.LogInformation("Schedule {ScheduleId} marked as available due to cancellation", 
                booking.ScheduleId);
        }

        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Booking {BookingId} status updated successfully", bookingId);
        return _mapper.Map<BookingDto>(booking);
    }

    public override async Task DeleteAsync(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Schedule)
            .Include(b => b.Review)
            .FirstOrDefaultAsync(b => b.BookingId == id) ?? throw new KeyNotFoundException($"Booking with id {id} not found");

        if (booking.Review is not null)
        {
            _logger.LogInformation("Deleting review {ReviewId} associated with booking {BookingId}", 
                booking.Review.ReviewId, id);
        }

        if (!booking.Schedule.IsAvailable)
        {
            booking.Schedule.IsAvailable = true;
            _logger.LogInformation("Schedule {ScheduleId} marked as available after booking deletion", 
                booking.ScheduleId);
        }

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Booking {BookingId} deleted successfully", id);
    }

    public async Task<IEnumerable<BookingDto>> GetByStudentAsync(int studentId)
    {
        var bookings = await _bookingRepository.GetByStudentIdAsync(studentId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetByTutorAsync(int tutorId)
    {
        var bookings = await _bookingRepository.GetByTutorIdAsync(tutorId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetByStatusAsync(BookingStatus status)
    {
        var bookings = await _bookingRepository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<(IEnumerable<BookingDto> Bookings, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        BookingStatus? status = null,
        int? studentId = null,
        int? tutorId = null)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
        }

        var query = _context.Bookings
            .Include(b => b.Student)
                .ThenInclude(s => s.User)
            .Include(b => b.TutorSubject)
                .ThenInclude(ts => ts.Tutor)
                    .ThenInclude(t => t.User)
            .Include(b => b.Schedule)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }

        if (studentId.HasValue)
        {
            query = query.Where(b => b.StudentId == studentId.Value);
        }

        if (tutorId.HasValue)
        {
            query = query.Where(b => b.TutorSubject.TutorId == tutorId.Value);
        }

        var totalCount = await query.CountAsync();

        var bookings = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var bookingDtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);

        return (bookingDtos, totalCount);
    }
}