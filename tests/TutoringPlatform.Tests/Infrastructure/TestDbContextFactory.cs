using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Enums;

namespace TutoringPlatform.Tests.Infrastructure;

public static class TestDbContextFactory
{
    public static TutoringDbContext CreateInMemoryContext(string databaseName = "")
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            databaseName = Guid.NewGuid().ToString();
        }

        var options = new DbContextOptionsBuilder<TutoringDbContext>()
            .UseInMemoryDatabase(databaseName)
            .EnableSensitiveDataLogging()
            .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new TutoringDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public static async Task SeedTestDataAsync(TutoringDbContext context)
    {
        var cities = new[]
        {
            new Models.Entities.City { CityId = 1, Name = "Kyiv" },
            new Models.Entities.City { CityId = 2, Name = "Lviv" },
            new Models.Entities.City { CityId = 3, Name = "Odessa" }
        };
        context.Cities.AddRange(cities);

        var subjects = new[]
        {
            new Models.Entities.Subject { SubjectId = 1, Name = "Mathematics", Category = "STEM" },
            new Models.Entities.Subject { SubjectId = 2, Name = "Physics", Category = "STEM" },
            new Models.Entities.Subject { SubjectId = 3, Name = "English", Category = "Language" }
        };
        context.Subjects.AddRange(subjects);

        var levels = new[]
        {
            new Models.Entities.TeachingLevel { LevelId = 1, Name = "Elementary", Position = 1 },
            new Models.Entities.TeachingLevel { LevelId = 2, Name = "High School", Position = 2 },
            new Models.Entities.TeachingLevel { LevelId = 3, Name = "College", Position = 3 }
        };
        context.TeachingLevels.AddRange(levels);

        var users = new[]
        {
            new Models.Entities.User 
            { 
                UserId = 1, 
                Email = "tutor1@test.com", 
                Phone = "1234567890",
                FirstName = "Dude",
                LastName = "Bro",
                UserType = UserType.Tutor
            },
            new Models.Entities.User 
            { 
                UserId = 2, 
                Email = "tutor2@test.com", 
                Phone = "1234567891",
                FirstName = "Dawg",
                LastName = "Man",
                UserType = UserType.Tutor
            },
            new Models.Entities.User 
            { 
                UserId = 3, 
                Email = "student1@test.com", 
                Phone = "1234567892",
                FirstName = "Gang",
                LastName = "Pal",
                UserType = UserType.Student
            },
            new Models.Entities.User 
            { 
                UserId = 4, 
                Email = "student2@test.com", 
                Phone = "1234567893",
                FirstName = "Buddy",
                LastName = "Vein",
                UserType = UserType.Student
            }
        };
        context.Users.AddRange(users);

        var tutors = new[]
        {
            new Models.Entities.Tutor 
            { 
                TutorId = 1, 
                CityId = 1,
                YearsExperience = 5,
                Education = "Master's in Mathematics",
                AboutMe = "Experienced math tutor",
                OnlineAvailable = true,
                OfflineAvailable = true,
                Address = "123 St"
            },
            new Models.Entities.Tutor 
            { 
                TutorId = 2, 
                CityId = 2,
                YearsExperience = 3,
                Education = "Bachelor's in Physics",
                AboutMe = "Physics enthusiast",
                OnlineAvailable = true,
                OfflineAvailable = false
            }
        };
        context.Tutors.AddRange(tutors);

        var students = new[]
        {
            new Models.Entities.Student 
            { 
                StudentId = 3, 
                CityId = 1,
                SchoolGrade = 10
            },
            new Models.Entities.Student 
            { 
                StudentId = 4, 
                CityId = 3,
                SchoolGrade = 12
            }
        };
        context.Students.AddRange(students);

        var tutorSubjects = new[]
        {
            new Models.Entities.TutorSubject 
            { 
                TutorSubjectId = 1,
                TutorId = 1, 
                SubjectId = 1, 
                LevelId = 2,
                HourlyRate = 50.00m
            },
            new Models.Entities.TutorSubject 
            { 
                TutorSubjectId = 2,
                TutorId = 1, 
                SubjectId = 1, 
                LevelId = 3,
                HourlyRate = 75.00m
            },
            new Models.Entities.TutorSubject 
            { 
                TutorSubjectId = 3,
                TutorId = 2, 
                SubjectId = 2, 
                LevelId = 2,
                HourlyRate = 60.00m
            }
        };
        context.TutorSubjects.AddRange(tutorSubjects);

        var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var dayAfter = DateOnly.FromDateTime(DateTime.Today.AddDays(2));
        
        var schedules = new[]
        {
            new Models.Entities.Schedule 
            { 
                ScheduleId = 1,
                TutorId = 1,
                Date = tomorrow,
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(11, 0),
                IsAvailable = true
            },
            new Models.Entities.Schedule 
            { 
                ScheduleId = 2,
                TutorId = 1,
                Date = dayAfter,
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 0),
                IsAvailable = true
            },
            new Models.Entities.Schedule 
            { 
                ScheduleId = 3,
                TutorId = 2,
                Date = tomorrow,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(11, 0),
                IsAvailable = true
            }
        };
        context.Schedules.AddRange(schedules);

        await context.SaveChangesAsync();
    }
}
