using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
	public void Configure(EntityTypeBuilder<Tutor> builder)
	{
		builder.ToTable("tutor");

		builder.HasKey(t => t.TutorId);
		builder.Property(t => t.TutorId).HasColumnName("tutor_id");

		builder.Property(t => t.CityId).HasColumnName("city_id");

		builder.Property(t => t.YearsExperience)
			.HasColumnName("years_experience")
			.IsRequired()
			.HasDefaultValue(0);

		builder.Property(t => t.Education)
			.HasColumnName("education")
			.IsRequired();

		builder.Property(t => t.AboutMe)
			.HasColumnName("about_me")
			.HasMaxLength(2000);

		builder.Property(t => t.OnlineAvailable)
			.HasColumnName("online_available")
			.IsRequired()
			.HasDefaultValue(true);

		builder.Property(t => t.OfflineAvailable)
			.HasColumnName("offline_available")
			.IsRequired()
			.HasDefaultValue(true);

		builder.Property(t => t.Address)
			.HasColumnName("address")
			.HasMaxLength(500);

		builder.HasOne(t => t.City)
			.WithMany(c => c.Tutors)
			.HasForeignKey(t => t.CityId)
			.HasConstraintName("tutor_city_fk")
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasMany(t => t.TutorSubjects)
			.WithOne(ts => ts.Tutor)
			.HasForeignKey(ts => ts.TutorId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(t => t.Schedules)
			.WithOne(s => s.Tutor)
			.HasForeignKey(s => s.TutorId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
