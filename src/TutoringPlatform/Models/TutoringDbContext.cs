using TutoringPlatform.Models.Configuration;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Models;

public class TutoringDbContext(DbContextOptions<TutoringDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<TeachingLevel> TeachingLevels { get; set; }
    public DbSet<TutorSubject> TutorSubjects { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (Database.IsNpgsql())
        {
            modelBuilder.HasPostgresEnum<UserType>();
            modelBuilder.HasPostgresEnum<BookingFormat>();
            modelBuilder.HasPostgresEnum<BookingStatus>();
        }

        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new TutorConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        modelBuilder.ApplyConfiguration(new TeachingLevelConfiguration());
        modelBuilder.ApplyConfiguration(new TutorSubjectConfiguration());
        modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
    }
}