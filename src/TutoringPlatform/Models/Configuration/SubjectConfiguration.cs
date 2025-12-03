using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
	public void Configure(EntityTypeBuilder<Subject> builder)
	{
		builder.ToTable("subject");

		builder.HasKey(s => s.SubjectId);
		builder.Property(s => s.SubjectId).HasColumnName("subject_id");

		builder.Property(s => s.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.HasIndex(s => s.Name).IsUnique();

		builder.Property(s => s.Category)
			.HasColumnName("category")
			.IsRequired();

		builder.Property(s => s.Description)
			.HasColumnName("description")
			.HasMaxLength(500);

		builder.HasMany(s => s.TutorSubjects)
			.WithOne(ts => ts.Subject)
			.HasForeignKey(ts => ts.SubjectId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
