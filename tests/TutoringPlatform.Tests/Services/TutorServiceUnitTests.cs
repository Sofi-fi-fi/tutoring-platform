using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Repositories.Interfaces;
using TutoringPlatform.Services;
using TutoringPlatform.Services.DTOs.Tutor;
using TutoringPlatform.Services.DTOs.User;
using TutoringPlatform.Tests.Infrastructure;

namespace TutoringPlatform.Tests.Services;

public class TutorServiceUnitTests
{
    private readonly Mock<ITutorRepository> _mockTutorRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ICityRepository> _mockCityRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TutorService>> _mockLogger;
    private readonly TutoringDbContext _context;
    private readonly TutorService _service;

    public TutorServiceUnitTests()
    {
        _mockTutorRepository = new Mock<ITutorRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockCityRepository = new Mock<ICityRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TutorService>>();
        _context = TestDbContextFactory.CreateInMemoryContext();

        _service = new TutorService(
            _mockTutorRepository.Object,
            _context,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockUserRepository.Object,
            _mockCityRepository.Object
        );
    }

    #region Validation Tests

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var createDto = new CreateTutorDto
        {
            User = new CreateUserDto
            {
                Email = "existing@test.com",
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.Tutor
            },
            YearsExperience = 5,
            Education = "Bachelor's",
            OnlineAvailable = true,
            OfflineAvailable = false
        };

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(createDto.User.Email))
            .ReturnsAsync(new User { UserId = 1, Email = createDto.User.Email });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("already exists", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenPhoneAlreadyExists()
    {
        var createDto = new CreateTutorDto
        {
            User = new CreateUserDto
            {
                Email = "new@test.com",
                Phone = "1234567890",
                FirstName = "Pal",
                LastName = "Dawg",
                UserType = UserType.Tutor
            },
            YearsExperience = 5,
            Education = "Bachelor's",
            OnlineAvailable = true,
            OfflineAvailable = false
        };

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(createDto.User.Email))
            .ReturnsAsync((User?)null);

        _mockUserRepository
            .Setup(r => r.GetByPhoneAsync(createDto.User.Phone))
            .ReturnsAsync(new User { UserId = 1, Phone = createDto.User.Phone });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("already exists", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenOfflineWithoutCity()
    {
        var createDto = new CreateTutorDto
        {
            User = new CreateUserDto
            {
                Email = "new@test.com",
                FirstName = "Pal",
                LastName = "Dawg",
                UserType = UserType.Tutor
            },
            YearsExperience = 5,
            Education = "Bachelor's",
            OnlineAvailable = false,
            OfflineAvailable = true,
            CityId = null,
            Address = "123 St"
        };

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(createDto.User.Email))
            .ReturnsAsync((User?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("city and address", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenOfflineWithoutAddress()
    {
        var createDto = new CreateTutorDto
        {
            User = new CreateUserDto
            {
                Email = "new@test.com",
                FirstName = "Pal",
                LastName = "Dawg",
                UserType = UserType.Tutor
            },
            YearsExperience = 5,
            Education = "Bachelor's",
            OnlineAvailable = false,
            OfflineAvailable = true,
            CityId = 1,
            Address = null
        };

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(createDto.User.Email))
            .ReturnsAsync((User?)null);

        _mockCityRepository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new City { CityId = 1, Name = "Test City" });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("city and address", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenNotAvailableOnlineOrOffline()
    {
        var createDto = new CreateTutorDto
        {
            User = new CreateUserDto
            {
                Email = "new@test.com",
                FirstName = "Pal",
                LastName = "Dawg",
                UserType = UserType.Tutor
            },
            YearsExperience = 5,
            Education = "Bachelor's",
            OnlineAvailable = false,
            OfflineAvailable = false
        };

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(createDto.User.Email))
            .ReturnsAsync((User?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("online or offline", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenUserTypeIsNotTutor()
    {
        var createDto = new CreateTutorDto
        {
            User = new CreateUserDto
            {
                Email = "new@test.com",
                FirstName = "Pal",
                LastName = "Dawg",
                UserType = UserType.Student
            },
            YearsExperience = 5,
            Education = "Bachelor's",
            OnlineAvailable = true,
            OfflineAvailable = false
        };

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(createDto.User.Email))
            .ReturnsAsync((User?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(createDto)
        );
        Assert.Contains("UserType must be Tutor", exception.Message);
    }

    #endregion
}
