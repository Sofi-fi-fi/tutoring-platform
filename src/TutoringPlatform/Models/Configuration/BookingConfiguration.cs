using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
	public void Configure(EntityTypeBuilder<Booking> builder)
	{
		builder.ToTable("booking");

		builder.HasKey(b => b.BookingId);
		builder.Property(b => b.BookingId)
			.HasColumnName("booking_id")
			.UseIdentityAlwaysColumn();

		builder.Property(b => b.StudentId)
			.HasColumnName("student_id")
			.IsRequired();

		builder.Property(b => b.TutorSubjectId)
			.HasColumnName("tutor_subject_id")
			.IsRequired();

		builder.Property(b => b.ScheduleId)
			.HasColumnName("schedule_id")
			.IsRequired();

		builder.Property(b => b.Format)
			.HasColumnName("format")
			.IsRequired();

		builder.Property(b => b.Status)
			.HasColumnName("status")
			.IsRequired()
			.HasDefaultValue(Enums.BookingStatus.Pending);

		builder.Property(b => b.CreatedAt)
			.HasColumnName("created_at")
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.HasIndex(b => b.ScheduleId).IsUnique();
		builder.HasIndex(b => b.StudentId).HasDatabaseName("idx_booking_student_id");

		builder.HasOne(b => b.Student)
			.WithMany(s => s.Bookings)
			.HasForeignKey(b => b.StudentId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(b => b.TutorSubject)
			.WithMany(ts => ts.Bookings)
			.HasForeignKey(b => b.TutorSubjectId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(b => b.Schedule)
			.WithOne(s => s.Booking)
			.HasForeignKey<Booking>(b => b.ScheduleId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
