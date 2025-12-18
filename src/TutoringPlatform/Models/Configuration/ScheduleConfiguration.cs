using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
	public void Configure(EntityTypeBuilder<Schedule> builder)
	{
		builder.ToTable("schedule", table =>
		{
			table.HasCheckConstraint("schedule_future_date", "date >= CURRENT_DATE");
			table.HasCheckConstraint("schedule_valid_time_range", "end_time > start_time");
			table.HasCheckConstraint("schedule_duration_60min", "EXTRACT(EPOCH from (end_time - start_time)) / 60 = 60");
		});

		builder.HasKey(s => s.ScheduleId);
		builder.Property(s => s.ScheduleId)
			.HasColumnName("schedule_id")
			.UseIdentityAlwaysColumn();

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
			.HasDatabaseName("idx_schedule_unique_slot");

		builder.HasIndex(s => new { s.TutorId, s.IsAvailable })
			.HasDatabaseName("idx_schedule_tutor_availability");

		builder.HasOne(s => s.Tutor)
			.WithMany(t => t.Schedules)
			.HasForeignKey(s => s.TutorId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
