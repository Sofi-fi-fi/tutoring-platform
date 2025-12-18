using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.ToTable("review", table =>
		{
			table.HasCheckConstraint("review_rating_range", "rating BETWEEN 1 AND 5");
		});

		builder.HasKey(r => r.ReviewId);
		builder.Property(r => r.ReviewId)
			.HasColumnName("review_id")
			.UseIdentityAlwaysColumn();

		builder.Property(r => r.BookingId)
			.HasColumnName("booking_id")
			.IsRequired();

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

		builder.HasIndex(r => r.BookingId).IsUnique();
		builder.HasIndex(r => r.Rating).HasDatabaseName("idx_review_rating");

		builder.HasOne(r => r.Booking)
			.WithOne(b => b.Review)
			.HasForeignKey<Review>(r => r.BookingId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
