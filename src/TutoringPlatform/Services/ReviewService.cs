using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Services.DTOs.Review;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public class ReviewService(
	IReviewRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<ReviewService> logger
) : BaseService<Review, ReviewDto, CreateReviewDto, UpdateReviewDto>(repository, context, mapper, logger), IReviewService
{
    private readonly IReviewRepository _reviewRepository = repository;

    public override async Task<ReviewDto> CreateAsync(CreateReviewDto createDto)
    {
        _logger.LogInformation("Creating review for booking {BookingId}", createDto.BookingId);

        var booking = await _context.Bookings
            .Include(b => b.Schedule)
            .FirstOrDefaultAsync(b => b.BookingId == createDto.BookingId) ?? throw new InvalidOperationException($"Booking with id {createDto.BookingId} does not exist");

        if (booking.Status != BookingStatus.Completed)
        {
            throw new InvalidOperationException("Can only create reviews for completed bookings");
        }

        if (booking.Schedule.Date >= DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Can only review past sessions");
        }

        var existingReview = await _reviewRepository.GetByBookingAsync(createDto.BookingId);
        if (existingReview is not null)
        {
            throw new InvalidOperationException($"Review already exists for booking {createDto.BookingId}");
        }

        var review = new Review
        {
            BookingId = createDto.BookingId,
            Rating = createDto.Rating,
            Comment = createDto.Comment,
            IsAnonymous = createDto.IsAnonymous,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Review created successfully. ReviewId: {ReviewId}", review.ReviewId);

        var createdReview = await _reviewRepository.GetByIdAsync(review.ReviewId);
        return _mapper.Map<ReviewDto>(createdReview);
    }

    public override async Task<ReviewDto> UpdateAsync(int id, UpdateReviewDto updateDto)
    {
        var review = await _context.Reviews
            .Include(r => r.Booking)
                .ThenInclude(b => b.Schedule)
            .FirstOrDefaultAsync(r => r.ReviewId == id)
            ?? throw new KeyNotFoundException($"Review with id {id} not found");

        if (review.Booking.Status != BookingStatus.Completed)
        {
            throw new InvalidOperationException("Can only update reviews for completed bookings");
        }

        if (review.Booking.Schedule.Date >= DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Can only update reviews after the session date has passed");
        }

        review.Rating = updateDto.Rating;
        review.Comment = updateDto.Comment;
        review.IsAnonymous = updateDto.IsAnonymous;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Review {ReviewId} updated successfully", id);
        return _mapper.Map<ReviewDto>(review);
    }

    public override async Task DeleteAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.ReviewId == id)
            ?? throw new KeyNotFoundException($"Review with id {id} not found");

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Review {ReviewId} deleted successfully", id);
    }

    public async Task<ReviewDto?> GetByBookingAsync(int bookingId)
    {
        var review = await _reviewRepository.GetByBookingAsync(bookingId);
        return review is null ? null : _mapper.Map<ReviewDto>(review);
    }

    public async Task<IEnumerable<ReviewDto>> GetByStudentAsync(int studentId)
    {
        var reviews = await _reviewRepository.GetByStudentIdAsync(studentId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<IEnumerable<ReviewDto>> GetByTutorAsync(int tutorId)
    {
        var reviews = await _reviewRepository.GetByTutorIdAsync(tutorId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<IEnumerable<ReviewDto>> GetByRatingAsync(short rating)
    {
        var reviews = await _reviewRepository.GetByRatingAsync(rating);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
}