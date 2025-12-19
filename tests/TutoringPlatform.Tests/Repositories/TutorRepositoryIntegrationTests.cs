using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;
using TutoringPlatform.Repositories;
using TutoringPlatform.Tests.Infrastructure;

namespace TutoringPlatform.Tests.Repositories;

public class TutorRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly TutorRepository _repository;

    public TutorRepositoryIntegrationTests()
    {
        _repository = new TutorRepository(Context);
    }

    #region Create Tests

    [Fact]
    public async Task AddAsync_ShouldCreateTutor_WhenValidData()
    {
        await SeedDatabaseAsync();
        
        var user = new User
        {
            UserId = 10,
            Email = "newtutor@test.com",
            Phone = "5555555555",
            FirstName = "New",
            LastName = "Tutor",
            UserType = UserType.Tutor
        };
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        
        var newTutor = new Tutor
        {
            TutorId = 10,
            CityId = 1,
            YearsExperience = 7,
            Education = "PhD in Computer Science",
            AboutMe = "Passionate about teaching",
            OnlineAvailable = true,
            OfflineAvailable = false
        };

        var result = await _repository.AddAsync(newTutor);

        Assert.NotNull(result);
        Assert.Equal(10, result.TutorId);
        Assert.Equal(7, result.YearsExperience);
        Assert.Equal("PhD in Computer Science", result.Education);

        var tutorFromDb = await _repository.GetByIdAsync(10);
        Assert.NotNull(tutorFromDb);
        Assert.NotNull(tutorFromDb.User);
        Assert.Equal("PhD in Computer Science", tutorFromDb.Education);
    }

    #endregion

    #region Read Tests

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTutorWithUser_WhenExists()
    {
        await SeedDatabaseAsync();

        var tutor = await _repository.GetByIdAsync(1);

        Assert.NotNull(tutor);
        Assert.Equal(1, tutor.TutorId);
        Assert.NotNull(tutor.User);
        Assert.Equal("tutor1@test.com", tutor.User.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        await SeedDatabaseAsync();

        var tutor = await _repository.GetByIdAsync(999);

        Assert.Null(tutor);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTutors_WithUsers()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.GetAllAsync();

        Assert.NotNull(tutors);
        var tutorList = tutors.ToList();
        Assert.Equal(2, tutorList.Count);
        Assert.All(tutorList, t => Assert.NotNull(t.User));
    }

    [Fact]
    public async Task GetByCityAsync_ShouldReturnTutorsInCity()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.GetByCityAsync(1);

        var tutorList = tutors.ToList();
        Assert.Single(tutorList);
        Assert.All(tutorList, t => Assert.Equal(1, t.CityId));
    }

    [Fact]
    public async Task GetByCityAsync_ShouldReturnEmpty_WhenNoCityTutors()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.GetByCityAsync(999);

        Assert.Empty(tutors);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task UpdateAsync_ShouldModifyTutor_WhenValidData()
    {
        await SeedDatabaseAsync();
        var tutor = await _repository.GetByIdAsync(1);
        Assert.NotNull(tutor);

        tutor.YearsExperience = 10;
        tutor.Education = "PhD in Mathematics";
        tutor.AboutMe = "Updated bio";
        await _repository.UpdateAsync(tutor);

        var updatedTutor = await _repository.GetByIdAsync(1);
        Assert.NotNull(updatedTutor);
        Assert.Equal(10, updatedTutor.YearsExperience);
        Assert.Equal("PhD in Mathematics", updatedTutor.Education);
        Assert.Equal("Updated bio", updatedTutor.AboutMe);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTutor_WhenExists()
    {
        await SeedDatabaseAsync();
        var tutor = await _repository.GetByIdAsync(2);
        Assert.NotNull(tutor);

        await _repository.DeleteAsync(tutor);

        var deletedTutor = await _repository.GetByIdAsync(2);
        Assert.Null(deletedTutor);

        var allTutors = await _repository.GetAllAsync();
        Assert.Single(allTutors);
    }

    #endregion

    #region Complex Query Tests

    [Fact]
    public async Task GetTopRatedTutors_ShouldReturnCorrectRankings()
    {
        await SeedDatabaseAsync();
        var repository = new TutorRepository(Context);

        var booking1 = new Booking
        {
            StudentId = 3,
            TutorSubjectId = 1,
            ScheduleId = 1,
            Format = BookingFormat.Online,
            Status = BookingStatus.Completed
        };
        Context.Bookings.Add(booking1);
        await Context.SaveChangesAsync();

        var review1 = new Review
        {
            BookingId = booking1.BookingId,
            Rating = 5,
            Comment = "Excellent tutor!"
        };
        Context.Reviews.Add(review1);

        var booking2 = new Booking
        {
            StudentId = 4,
            TutorSubjectId = 2,
            ScheduleId = 2,
            Format = BookingFormat.Online,
            Status = BookingStatus.Completed
        };
        Context.Bookings.Add(booking2);
        await Context.SaveChangesAsync();

        var review2 = new Review
        {
            BookingId = booking2.BookingId,
            Rating = 4,
            Comment = "Very good!"
        };
        Context.Reviews.Add(review2);

        var schedule1 = await Context.Schedules.FindAsync(1);
        schedule1!.Booking = booking1;
        var schedule2 = await Context.Schedules.FindAsync(2);
        schedule2!.Booking = booking2;
        await Context.SaveChangesAsync();

        var topTutors = await repository.GetTopRatedAsync(5);

        var tutorList = topTutors.ToList();
        Assert.NotEmpty(tutorList);
        
        var topTutor = tutorList.First();
        Assert.Equal(1, topTutor.Tutor.TutorId);
        Assert.Equal(4.5, topTutor.AverageRating);
    }
    
    [Fact]
    public async Task SearchTutorsAsync_ShouldFilterByCity()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.SearchTutorsAsync(
            cityId: 1, 
            subjectId: null, 
            levelId: null, 
            minPrice: null, 
            maxPrice: null, 
            onlineOnly: null, 
            offlineOnly: null
        );

        var tutorList = tutors.ToList();
        Assert.Single(tutorList);
        Assert.All(tutorList, t => Assert.Equal(1, t.CityId));
    }

    [Fact]
    public async Task SearchTutorsAsync_ShouldFilterBySubject()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.SearchTutorsAsync(
            cityId: null, 
            subjectId: 1,
            levelId: null, 
            minPrice: null, 
            maxPrice: null, 
            onlineOnly: null, 
            offlineOnly: null
        );

        var tutorList = tutors.ToList();
        Assert.Single(tutorList);
        Assert.Equal(1, tutorList[0].TutorId);
    }

    [Fact]
    public async Task SearchTutorsAsync_ShouldFilterByPriceRange()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.SearchTutorsAsync(
            cityId: null, 
            subjectId: null, 
            levelId: null, 
            minPrice: 40m, 
            maxPrice: 60m, 
            onlineOnly: null, 
            offlineOnly: null
        );

        var tutorList = tutors.ToList();
        Assert.Equal(2, tutorList.Count);
    }

    [Fact]
    public async Task SearchTutorsAsync_ShouldFilterByOnlineAvailability()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.SearchTutorsAsync(
            cityId: null, 
            subjectId: null, 
            levelId: null, 
            minPrice: null, 
            maxPrice: null, 
            onlineOnly: true, 
            offlineOnly: null
        );

        var tutorList = tutors.ToList();
        Assert.Equal(2, tutorList.Count);
        Assert.All(tutorList, t => Assert.True(t.OnlineAvailable));
    }

    [Fact]
    public async Task SearchTutorsAsync_ShouldFilterByOfflineAvailability()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.SearchTutorsAsync(
            cityId: null, 
            subjectId: null, 
            levelId: null, 
            minPrice: null, 
            maxPrice: null, 
            onlineOnly: null, 
            offlineOnly: true
        );

        var tutorList = tutors.ToList();
        Assert.Single(tutorList);
        Assert.All(tutorList, t => Assert.True(t.OfflineAvailable));
    }

    [Fact]
    public async Task SearchTutorsAsync_ShouldCombineMultipleFilters()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.SearchTutorsAsync(
            cityId: 1, 
            subjectId: 1, 
            levelId: 2, 
            minPrice: 40m, 
            maxPrice: 60m, 
            onlineOnly: true, 
            offlineOnly: null
        );

        var tutorList = tutors.ToList();
        Assert.Single(tutorList);
        Assert.Equal(1, tutorList[0].TutorId);
    }

    [Fact]
    public async Task SearchTutorsAsync_ShouldReturnEmpty_WhenNoMatches()
    {
        await SeedDatabaseAsync();

        var tutors = await _repository.SearchTutorsAsync(
            cityId: 999, 
            subjectId: null, 
            levelId: null, 
            minPrice: null, 
            maxPrice: null, 
            onlineOnly: null, 
            offlineOnly: null
        );

        Assert.Empty(tutors);
    }

    #endregion
}
