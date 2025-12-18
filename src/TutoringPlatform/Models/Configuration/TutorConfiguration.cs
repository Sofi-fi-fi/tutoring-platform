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
			.HasDefaultValue((short)0);

		builder.Property(t => t.Education)
			.HasColumnName("education")
			.HasColumnType("text")
			.IsRequired();

		builder.Property(t => t.AboutMe)
			.HasColumnName("about_me")
			.HasColumnType("text");

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
			.HasColumnType("text");

		builder.HasIndex(t => t.CityId).HasDatabaseName("idx_tutor_city_id");

		builder.HasOne(t => t.City)
			.WithMany(c => c.Tutors)
			.HasForeignKey(t => t.CityId)
			.HasConstraintName("tutor_city_fk")
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasOne(t => t.User)
			.WithOne(u => u.Tutor)
			.HasForeignKey<Tutor>(t => t.TutorId)
			.HasConstraintName("tutor_user_fk")
			.OnDelete(DeleteBehavior.Cascade);
	}
}
