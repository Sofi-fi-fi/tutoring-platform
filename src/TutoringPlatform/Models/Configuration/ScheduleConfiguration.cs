using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
	public void Configure(EntityTypeBuilder<Schedule> builder)
	{
		builder.ToTable("schedule");

		builder.HasKey(s => s.ScheduleId);
		builder.Property(s => s.ScheduleId).HasColumnName("schedule_id");

		builder.Property(s => s.TutorId)
			.HasColumnName("tutor_id")
			.IsRequired();

		builder.Property(s => s.Date)
			.HasColumnName("date")
			.IsRequired();

		builder.Property(s => s.StartTime)
			.HasColumnName("start_time")
			.IsRequired();

		builder.Property(s => s.EndTime)
			.HasColumnName("end_time")
			.IsRequired();

		builder.Property(s => s.IsAvailable)
			.HasColumnName("is_available")
			.IsRequired()
			.HasDefaultValue(true);

		builder.Property(s => s.CreatedAt)
			.HasColumnName("created_at")
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.HasIndex(s => new { s.TutorId, s.Date, s.StartTime, s.EndTime })
			.IsUnique()
			.HasDatabaseName("schedule_unique_slot");

		builder.HasOne(s => s.Booking)
			.WithOne(b => b.Schedule)
			.HasForeignKey<Booking>(b => b.ScheduleId);
	}
}
