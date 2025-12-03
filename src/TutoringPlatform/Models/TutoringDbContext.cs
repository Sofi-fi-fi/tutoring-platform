using System;
using Microsoft.EntityFrameworkCore;
using TutoringPlatform.Models.Configuration;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models;

public class TutoringDbContext : DbContext
{
    public TutoringDbContext(DbContextOptions<TutoringDbContext> options)
        : base(options)
    {
    }

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

        modelBuilder.HasPostgresEnum<Enums.UserType>();
        modelBuilder.HasPostgresEnum<Enums.SubjectCategory>();
        modelBuilder.HasPostgresEnum<Enums.BookingFormat>();
        modelBuilder.HasPostgresEnum<Enums.BookingStatus>();
    }
}