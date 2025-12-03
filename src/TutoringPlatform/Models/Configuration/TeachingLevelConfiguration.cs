using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class TeachingLevelConfiguration : IEntityTypeConfiguration<TeachingLevel>
{
	public void Configure(EntityTypeBuilder<TeachingLevel> builder)
	{
		builder.ToTable("teaching_level");

		builder.HasKey(tl => tl.LevelId);
		builder.Property(tl => tl.LevelId)
			.HasColumnName("level_id")
			.UseIdentityAlwaysColumn();

		builder.Property(tl => tl.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.HasIndex(tl => tl.Name).IsUnique();

		builder.Property(tl => tl.Position)
			.HasColumnName("position")
			.IsRequired();

		builder.HasIndex(tl => tl.Position).IsUnique();

		builder.Property(tl => tl.Description)
			.HasColumnName("description")
			.HasColumnType("text");

		builder.HasMany(tl => tl.TutorSubjects)
			.WithOne(ts => ts.TeachingLevel)
			.HasForeignKey(ts => ts.LevelId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
