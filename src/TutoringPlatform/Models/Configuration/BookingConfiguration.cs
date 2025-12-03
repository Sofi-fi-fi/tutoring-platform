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

		builder.HasIndex(b => b.ScheduleId).IsUnique();

		builder.Property(b => b.Format)
			.HasColumnName("format")
			.HasConversion<string>()
			.IsRequired();

		builder.Property(b => b.Status)
			.HasColumnName("status")
			.HasConversion<string>()
			.IsRequired()
			.HasDefaultValue(Enums.BookingStatus.Pending);

		builder.Property(b => b.CreatedAt)
			.HasColumnName("created_at")
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.HasOne(b => b.Review)
			.WithOne(r => r.Booking)
			.HasForeignKey<Review>(r => r.BookingId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
