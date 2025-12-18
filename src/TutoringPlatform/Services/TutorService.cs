using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.DTOs.Tutor;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public class TutorService(
	ITutorRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<TutorService> logger,
	IUserRepository userRepository,
	ICityRepository cityRepository) 
: BaseService<Tutor, TutorDto, CreateTutorDto, UpdateTutorDto>(repository, context, mapper, logger), ITutorService
{
    private readonly ITutorRepository _tutorRepository = repository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICityRepository _cityRepository = cityRepository;

    public override async Task<TutorDto> CreateAsync(CreateTutorDto createDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            _logger.LogInformation("Creating tutor with user. Email: {Email}", createDto.User.Email);

            var existingUserByEmail = await _userRepository.GetByEmailAsync(createDto.User.Email);
            if (existingUserByEmail is not null)
            {
                throw new InvalidOperationException($"User with email {createDto.User.Email} already exists");
            }

            if (!string.IsNullOrEmpty(createDto.User.Phone))
            {
                var existingUserByPhone = await _userRepository.GetByPhoneAsync(createDto.User.Phone);
                if (existingUserByPhone is not null)
                {
                    throw new InvalidOperationException($"User with phone {createDto.User.Phone} already exists");
                }
            }

            if (createDto.CityId.HasValue)
            {
                var city = await _cityRepository.GetByIdAsync(createDto.CityId.Value) ?? throw new InvalidOperationException($"City with id {createDto.CityId.Value} does not exist");
			}

			if (createDto.OfflineAvailable && (createDto.CityId == null || string.IsNullOrWhiteSpace(createDto.Address)))
            {
                throw new InvalidOperationException("Offline tutors must have a city and address");
            }

            if (!createDto.OnlineAvailable && !createDto.OfflineAvailable)
            {
                throw new InvalidOperationException("Tutor must be available either online or offline");
            }

            if (createDto.User.UserType != UserType.Tutor)
            {
                throw new InvalidOperationException("UserType must be Tutor when creating a tutor");
            }

            var user = new User
            {
                FirstName = createDto.User.FirstName,
                LastName = createDto.User.LastName,
                Email = createDto.User.Email,
                Phone = createDto.User.Phone,
                UserType = UserType.Tutor,
                DateOfBirth = createDto.User.DateOfBirth,
                RegistrationDate = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User created with ID: {UserId}", user.UserId);

            var tutor = new Tutor
            {
                TutorId = user.UserId,
                CityId = createDto.CityId,
                YearsExperience = createDto.YearsExperience,
                Education = createDto.Education,
                AboutMe = createDto.AboutMe,
                OnlineAvailable = createDto.OnlineAvailable,
                OfflineAvailable = createDto.OfflineAvailable,
                Address = createDto.Address
            };

            await _context.Tutors.AddAsync(tutor);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Tutor created successfully. TutorId: {TutorId}", tutor.TutorId);

            var createdTutor = await _tutorRepository.GetByIdAsync(tutor.TutorId);
            return _mapper.Map<TutorDto>(createdTutor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tutor with user. Rolling back transaction");
            throw;
        }
    }

    public override async Task<TutorDto> UpdateAsync(int id, UpdateTutorDto updateDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            _logger.LogInformation("Updating tutor {TutorId}", id);

            var tutor = await _context.Tutors
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TutorId == id)
                ?? throw new KeyNotFoundException($"Tutor with id {id} not found");

            if (tutor.User.Email != updateDto.User.Email)
            {
                var existingUserByEmail = await _userRepository.GetByEmailAsync(updateDto.User.Email);
                if (existingUserByEmail is not null && existingUserByEmail.UserId != tutor.User.UserId)
                {
                    throw new InvalidOperationException($"User with email {updateDto.User.Email} already exists");
                }
            }

            if (!string.IsNullOrEmpty(updateDto.User.Phone) && tutor.User.Phone != updateDto.User.Phone)
            {
                var existingUserByPhone = await _userRepository.GetByPhoneAsync(updateDto.User.Phone);
                if (existingUserByPhone is not null && existingUserByPhone.UserId != tutor.User.UserId)
                {
                    throw new InvalidOperationException($"User with phone {updateDto.User.Phone} already exists");
                }
            }

            if (updateDto.CityId.HasValue && updateDto.CityId != tutor.CityId)
            {
                var city = await _cityRepository.GetByIdAsync(updateDto.CityId.Value) 
                    ?? throw new InvalidOperationException($"City with id {updateDto.CityId.Value} does not exist");
            }

            if (updateDto.OfflineAvailable && (updateDto.CityId == null || string.IsNullOrWhiteSpace(updateDto.Address)))
            {
                throw new InvalidOperationException("Offline tutors must have a city and address");
            }

            if (!updateDto.OnlineAvailable && !updateDto.OfflineAvailable)
            {
                throw new InvalidOperationException("Tutor must be available either online or offline");
            }

            tutor.User.FirstName = updateDto.User.FirstName;
            tutor.User.LastName = updateDto.User.LastName;
            tutor.User.Email = updateDto.User.Email;
            tutor.User.Phone = updateDto.User.Phone;

            tutor.CityId = updateDto.CityId;
            tutor.YearsExperience = updateDto.YearsExperience;
            tutor.Education = updateDto.Education;
            tutor.AboutMe = updateDto.AboutMe;
            tutor.OnlineAvailable = updateDto.OnlineAvailable;
            tutor.OfflineAvailable = updateDto.OfflineAvailable;
            tutor.Address = updateDto.Address;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Tutor {TutorId} updated successfully", id);

            var updatedTutor = await _tutorRepository.GetByIdAsync(id);
            return _mapper.Map<TutorDto>(updatedTutor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tutor {TutorId}. Rolling back transaction", id);
            throw;
        }
    }

    public override async Task DeleteAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var tutor = await _context.Tutors
                .Include(t => t.User)
                .Include(t => t.TutorSubjects)
                .Include(t => t.Schedules)
                    .ThenInclude(s => s.Booking)
                .FirstOrDefaultAsync(t => t.TutorId == id) 
                ?? throw new KeyNotFoundException($"Tutor with id {id} not found");

            _logger.LogInformation("Deleting tutor {TutorId} and associated user {UserId}", 
                id, tutor.User.UserId);

            if (tutor.TutorSubjects.Count is not 0)
            {
                _logger.LogWarning("Tutor {TutorId} has {SubjectCount} tutor-subject relationship(s). These will be deleted.", 
                    id, tutor.TutorSubjects.Count);
            }

            if (tutor.Schedules.Count is not 0)
            {
                _logger.LogWarning("Tutor {TutorId} has {ScheduleCount} schedule(s). These will be deleted.", 
                    id, tutor.Schedules.Count);
            }

            var bookingsCount = tutor.Schedules.Count(s => s.Booking is not null);
            if (bookingsCount > 0)
            {
                _logger.LogWarning("Tutor {TutorId} has {BookingCount} booking(s) through schedules. These will be deleted.", 
                    id, bookingsCount);
            }

            _context.Users.Remove(tutor.User);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Tutor {TutorId} and user {UserId} deleted successfully", 
                id, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tutor {TutorId}. Rolling back transaction", id);
            throw;
        }
    }

    public override async Task<TutorDto?> GetByIdAsync(int tutorId)
    {
        var tutor = await _tutorRepository.GetByIdAsync(tutorId);
        return tutor is null ? null : _mapper.Map<TutorDto>(tutor);
    }

    public override async Task<IEnumerable<TutorDto>> GetAllAsync()
    {
        var tutors = await _tutorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TutorDto>>(tutors);
    }

    public async Task<IEnumerable<TutorDto>> GetByCityAsync(int cityId)
    {
        var tutors = await _tutorRepository.GetByCityAsync(cityId);
        return _mapper.Map<IEnumerable<TutorDto>>(tutors);
    }

    public async Task<(IEnumerable<TutorDto> Tutors, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string? sortBy = null,
        bool descending = false)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
        }

        var query = _context.Tutors
            .Include(t => t.User)
            .Include(t => t.City)
            .AsQueryable();

        query = sortBy?.ToLower() switch
        {
            "experience" => descending 
                ? query.OrderByDescending(t => t.YearsExperience)
                : query.OrderBy(t => t.YearsExperience),
            "name" => descending
                ? query.OrderByDescending(t => t.User.LastName).ThenByDescending(t => t.User.FirstName)
                : query.OrderBy(t => t.User.LastName).ThenBy(t => t.User.FirstName),
            _ => query.OrderBy(t => t.TutorId)
        };

        var totalCount = await query.CountAsync();
        var tutors = await query
            .OrderBy(t => t.TutorId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var tutorDtos = _mapper.Map<IEnumerable<TutorDto>>(tutors);
        return (tutorDtos, totalCount);
    }

    public async Task<IEnumerable<TutorDto>> SearchTutorsAsync(TutorSearchDto searchDto)
    {
        var tutors = await _tutorRepository.SearchTutorsAsync(searchDto.CityID, searchDto.SubjectId, searchDto.LevelId, searchDto.MinPrice, searchDto.MaxPrice, searchDto.OnlineOnly, searchDto.OfflineOnly);
        return _mapper.Map<IEnumerable<TutorDto>>(tutors);
    }

    public async Task<IEnumerable<TutorTopRatedDto>> GetTopRatedAsync(int count)
    {
        var tutors = await _tutorRepository.GetTopRatedAsync(count);
        return _mapper.Map<IEnumerable<TutorTopRatedDto>>(tutors);
    }
}