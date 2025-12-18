using TutoringPlatform.Models.Enums;
using TutoringPlatform.Services.DTOs.Booking;

namespace TutoringPlatform.Services.Interfaces;

public interface IBookingService : IBaseService<BookingDto, CreateBookingDto, UpdateBookingDto>
{
    Task<IEnumerable<BookingDto>> GetByStudentAsync(int studentId);
    Task<IEnumerable<BookingDto>> GetByTutorAsync(int tutorId);
    Task<IEnumerable<BookingDto>> GetByStatusAsync(BookingStatus status);
    Task<(IEnumerable<BookingDto> Bookings, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, BookingStatus? status = null, int? studentId = null, int? tutorId = null);
}