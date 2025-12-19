using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Repositories.Interfaces;
using TutoringPlatform.Services;
using TutoringPlatform.Services.DTOs.Booking;
using TutoringPlatform.Tests.Infrastructure;

namespace TutoringPlatform.Tests.Services;

public class BookingServiceUnitTests : IntegrationTestBase
{
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<BookingService>> _mockLogger;
    private readonly BookingService _service;

    public BookingServiceUnitTests()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<BookingService>>();

        _service = new BookingService(
            _mockBookingRepository.Object,
            Context,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockStudentRepository.Object
        );
    }

    #region Validation Tests

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenStudentDoesNotExist()
    {
        var createDto = new CreateBookingDto
        {
            StudentId = 999,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online
        };

        _mockStudentRepository
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Student?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("does not exist", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenTutorSubjectDoesNotExist()
    {
        await SeedDatabaseAsync();
        
        var createDto = new CreateBookingDto
        {
            StudentId = 3,
            TutorSubjectId = 999,
            ScheduleId = 1,
            Format = BookingFormat.Online
        };

        _mockStudentRepository
            .Setup(r => r.GetByIdAsync(3))
            .ReturnsAsync(new Student { StudentId = 3 });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("does not exist", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenScheduleDoesNotExist()
    {
        await SeedDatabaseAsync();
        
        var createDto = new CreateBookingDto
        {
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 999,
            Format = BookingFormat.Online
        };

        _mockStudentRepository
            .Setup(r => r.GetByIdAsync(3))
            .ReturnsAsync(new Student { StudentId = 3 });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("does not exist", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenOnlineNotAvailable()
    {
        await SeedDatabaseAsync();
        
        var offlineTutor = new Tutor
        {
            TutorId = 5,
            YearsExperience = 3,
            Education = "Test",
            OnlineAvailable = false,
            OfflineAvailable = true
        };
        Context.Tutors.Add(offlineTutor);

        var tutorSubject = new TutorSubject
        {
            TutorSubjectId = 10,
            TutorId = 5,
            SubjectId = 1,
            LevelId = 1,
            HourlyRate = 50m
        };
        Context.TutorSubjects.Add(tutorSubject);

        var schedule = new Schedule
        {
            ScheduleId = 10,
            TutorId = 5,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0),
            IsAvailable = true
        };
        Context.Schedules.Add(schedule);
        await Context.SaveChangesAsync();

        var createDto = new CreateBookingDto
        {
            StudentId = 3,
            TutorSubjectId = 10,
            ScheduleId = 10,
            Format = BookingFormat.Online
        };

        _mockStudentRepository
            .Setup(r => r.GetByIdAsync(3))
            .ReturnsAsync(new Student { StudentId = 3 });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("not available for online", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenOfflineNotAvailable()
    {
        await SeedDatabaseAsync();
                var createDto = new CreateBookingDto
        {
            StudentId = 3,
            TutorSubjectId = 3,
            ScheduleId = 3,
            Format = BookingFormat.Offline
        };

        _mockStudentRepository
            .Setup(r => r.GetByIdAsync(3))
            .ReturnsAsync(new Student { StudentId = 3 });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("not available for offline", exception.Message);
    }

    #endregion
}
