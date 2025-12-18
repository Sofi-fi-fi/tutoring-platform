using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Services.DTOs.Student;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public class StudentService(
	IStudentRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<StudentService> logger,
	IUserRepository userRepository,
	ICityRepository cityRepository
) : BaseService<Student, StudentDto, CreateStudentDto, UpdateStudentDto>(repository, context, mapper, logger), IStudentService
{
    private readonly IStudentRepository _studentRepository = repository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICityRepository _cityRepository = cityRepository;

    public override async Task<StudentDto> CreateAsync(CreateStudentDto createDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            _logger.LogInformation("Creating student with user. Email: {Email}", createDto.User.Email);

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

			if (createDto.User.UserType != UserType.Student)
            {
                throw new InvalidOperationException("UserType must be Student when creating a student");
            }

            var user = new User
            {
                FirstName = createDto.User.FirstName,
                LastName = createDto.User.LastName,
                Email = createDto.User.Email,
                Phone = createDto.User.Phone,
                UserType = UserType.Student,
                DateOfBirth = createDto.User.DateOfBirth,
                RegistrationDate = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User created with ID: {UserId}", user.UserId);

            var student = new Student
            {
                StudentId = user.UserId,
                CityId = createDto.CityId,
                SchoolGrade = createDto.SchoolGrade
            };

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Student created successfully. StudentId: {StudentId}", student.StudentId);

            var createdStudent = await _studentRepository.GetByIdAsync(student.StudentId);
            return _mapper.Map<StudentDto>(createdStudent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student with user. Rolling back transaction");
            throw;
        }
    }

    public override async Task<StudentDto> UpdateAsync(int id, UpdateStudentDto updateDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            _logger.LogInformation("Updating student {StudentId}", id);

            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == id)
                ?? throw new KeyNotFoundException($"Student with id {id} not found");

            if (student.User.Email != updateDto.User.Email)
            {
                var existingUserByEmail = await _userRepository.GetByEmailAsync(updateDto.User.Email);
                if (existingUserByEmail is not null && existingUserByEmail.UserId != student.User.UserId)
                {
                    throw new InvalidOperationException($"User with email {updateDto.User.Email} already exists");
                }
            }

            if (!string.IsNullOrEmpty(updateDto.User.Phone) && student.User.Phone != updateDto.User.Phone)
            {
                var existingUserByPhone = await _userRepository.GetByPhoneAsync(updateDto.User.Phone);
                if (existingUserByPhone is not null && existingUserByPhone.UserId != student.User.UserId)
                {
                    throw new InvalidOperationException($"User with phone {updateDto.User.Phone} already exists");
                }
            }

            if (updateDto.CityId.HasValue && updateDto.CityId != student.CityId)
            {
                var city = await _cityRepository.GetByIdAsync(updateDto.CityId.Value) 
                    ?? throw new InvalidOperationException($"City with id {updateDto.CityId.Value} does not exist");
            }

            student.User.FirstName = updateDto.User.FirstName;
            student.User.LastName = updateDto.User.LastName;
            student.User.Email = updateDto.User.Email;
            student.User.Phone = updateDto.User.Phone;

            student.CityId = updateDto.CityId;
            student.SchoolGrade = updateDto.SchoolGrade;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Student {StudentId} updated successfully", id);

            var updatedStudent = await _studentRepository.GetByIdAsync(id);
            return _mapper.Map<StudentDto>(updatedStudent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student {StudentId}. Rolling back transaction", id);
            throw;
        }
    }

    public override async Task DeleteAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(s => s.StudentId == id) 
                ?? throw new KeyNotFoundException($"Student with id {id} not found");

            _logger.LogInformation("Deleting student {StudentId} and associated user {UserId}", 
                id, student.User.UserId);

            if (student.Bookings.Count is not 0)
            {
                _logger.LogWarning("Student {StudentId} has {BookingCount} booking(s). These will be deleted.", 
                    id, student.Bookings.Count);
            }

            _context.Users.Remove(student.User);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Student {StudentId} and user {UserId} deleted successfully", 
                id, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student {StudentId}. Rolling back transaction", id);
            throw;
        }
    }

    public override async Task<StudentDto?> GetByIdAsync(int studentId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        return student is null ? null : _mapper.Map<StudentDto>(student);
    }

    public override async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<IEnumerable<StudentDto>> GetByCityAsync(int cityId)
    {
        var students = await _studentRepository.GetByCityIdAsync(cityId);
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<(IEnumerable<StudentDto> Students, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        int? cityId = null,
        short? schoolGrade = null)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
        }

        var query = _context.Students
            .Include(s => s.User)
            .Include(s => s.City)
            .AsQueryable();

        if (cityId.HasValue)
        {
            query = query.Where(s => s.CityId == cityId.Value);
        }

        if (schoolGrade.HasValue)
        {
            query = query.Where(s => s.SchoolGrade == schoolGrade.Value);
        }

        var totalCount = await query.CountAsync();
        var students = await query
            .OrderBy(s => s.StudentId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
        return (studentDtos, totalCount);
    }
}