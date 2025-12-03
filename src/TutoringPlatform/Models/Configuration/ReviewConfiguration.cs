using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.ToTable("review");

		builder.HasKey(r => r.ReviewId);
		builder.Property(r => r.ReviewId).HasColumnName("review_id");

		builder.Property(r => r.BookingId)
			.HasColumnName("booking_id")
			.IsRequired();

		builder.HasIndex(r => r.BookingId).IsUnique();

		builder.Property(r => r.Rating)
			.HasColumnName("rating")
			.IsRequired();

		builder.Property(r => r.Comment)
			.HasColumnName("comment")
			.HasColumnType("text");

		builder.Property(r => r.CreatedAt)
			.HasColumnName("created_at")
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.Property(r => r.IsAnonymous)
			.HasColumnName("is_anonymous")
			.IsRequired()
			.HasDefaultValue(false);
	}
}
