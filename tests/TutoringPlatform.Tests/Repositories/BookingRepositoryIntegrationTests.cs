using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Repositories;
using TutoringPlatform.Tests.Infrastructure;

namespace TutoringPlatform.Tests.Repositories;

public class BookingRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly BookingRepository _repository;

    public BookingRepositoryIntegrationTests()
    {
        _repository = new BookingRepository(Context);
    }

    #region Create Tests

    [Fact]
    public async Task AddAsync_ShouldCreateBooking_WhenValidData()
    {
        await SeedDatabaseAsync();
        var booking = new Booking
        {
            BookingId = 10,
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };

        var result = await _repository.AddAsync(booking);

        Assert.NotNull(result);
        Assert.Equal(10, result.BookingId);
        Assert.Equal(BookingFormat.Online, result.Format);
        Assert.Equal(BookingStatus.Pending, result.Status);

        var bookingFromDb = await _repository.GetByIdAsync(10);
        Assert.NotNull(bookingFromDb);
    }

    #endregion

    #region Read Tests

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBooking_WhenExists()
    {
        await SeedDatabaseAsync();
        var booking = new Booking
        {
            BookingId = 1,
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Confirmed
        };
        await _repository.AddAsync(booking);

        var result = await _repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.BookingId);
        Assert.Equal(BookingStatus.Confirmed, result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        await SeedDatabaseAsync();

        var result = await _repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByStudentIdAsync_ShouldReturnStudentBookings()
    {
        await SeedDatabaseAsync();
        
        var booking1 = new Booking
        {
            BookingId = 1,
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };
        var booking2 = new Booking
        {
            BookingId = 2,
            StudentId = 3,
            TutorSubjectId = 2,
            ScheduleId = 2,
            Format = BookingFormat.Offline,
            Status = BookingStatus.Confirmed
        };
        var booking3 = new Booking
        {
            BookingId = 3,
            StudentId = 4,
            TutorSubjectId = 1,
            ScheduleId = 3,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };

        await _repository.AddAsync(booking1);
        await _repository.AddAsync(booking2);
        await _repository.AddAsync(booking3);

        var bookings = await _repository.GetByStudentIdAsync(3);

        var bookingList = bookings.ToList();
        Assert.Equal(2, bookingList.Count);
        Assert.All(bookingList, b => Assert.Equal(3, b.StudentId));
    }

    [Fact]
    public async Task GetByTutorIdAsync_ShouldReturnTutorBookings()
    {
        await SeedDatabaseAsync();
        
        var booking1 = new Booking
        {
            BookingId = 1,
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };
        var booking2 = new Booking
        {
            BookingId = 2,
            StudentId = 4,
            TutorSubjectId = 2,
            ScheduleId = 2,
            Format = BookingFormat.Offline,
            Status = BookingStatus.Confirmed
        };
        var booking3 = new Booking
        {
            BookingId = 3,
            StudentId = 3,
            TutorSubjectId = 3,
            ScheduleId = 3,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };

        await _repository.AddAsync(booking1);
        await _repository.AddAsync(booking2);
        await _repository.AddAsync(booking3);

        var bookings = await _repository.GetByTutorIdAsync(1);

        var bookingList = bookings.ToList();
        Assert.Equal(2, bookingList.Count);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnBookingsWithStatus()
    {
        await SeedDatabaseAsync();
        
        var booking1 = new Booking
        {
            BookingId = 1,
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };
        var booking2 = new Booking
        {
            BookingId = 2,
            StudentId = 4,
            TutorSubjectId = 2,
            ScheduleId = 2,
            Format = BookingFormat.Offline,
            Status = BookingStatus.Confirmed
        };
        var booking3 = new Booking
        {
            BookingId = 3,
            StudentId = 3,
            TutorSubjectId = 3,
            ScheduleId = 3,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };

        await _repository.AddAsync(booking1);
        await _repository.AddAsync(booking2);
        await _repository.AddAsync(booking3);

        var bookings = await _repository.GetByStatusAsync(BookingStatus.Pending);

        var bookingList = bookings.ToList();
        Assert.Equal(2, bookingList.Count);
        Assert.All(bookingList, b => Assert.Equal(BookingStatus.Pending, b.Status));
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task UpdateAsync_ShouldModifyBooking_WhenValidData()
    {
        await SeedDatabaseAsync();
        var booking = new Booking
        {
            BookingId = 1,
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };
        await _repository.AddAsync(booking);

        booking.Status = BookingStatus.Confirmed;
        booking.Format = BookingFormat.Offline;
        await _repository.UpdateAsync(booking);

        var updatedBooking = await _repository.GetByIdAsync(1);
        Assert.NotNull(updatedBooking);
        Assert.Equal(BookingStatus.Confirmed, updatedBooking.Status);
        Assert.Equal(BookingFormat.Offline, updatedBooking.Format);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task DeleteAsync_ShouldRemoveBooking_WhenExists()
    {
        await SeedDatabaseAsync();
        var booking = new Booking
        {
            BookingId = 1,
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Pending
        };
        await _repository.AddAsync(booking);

        await _repository.DeleteAsync(booking);

        var deletedBooking = await _repository.GetByIdAsync(1);
        Assert.Null(deletedBooking);
    }

    #endregion
}
