using TutoringPlatform.Services.DTOs.Review;

namespace TutoringPlatform.Services.Interfaces;

public interface IReviewService : IBaseService<ReviewDto, CreateReviewDto, UpdateReviewDto>
{   
    Task<ReviewDto?> GetByBookingAsync(int bookingId);
    Task<IEnumerable<ReviewDto>> GetByStudentAsync(int studentId);
    Task<IEnumerable<ReviewDto>> GetByTutorAsync(int tutorId);
    Task<IEnumerable<ReviewDto>> GetByRatingAsync(short rating);
}