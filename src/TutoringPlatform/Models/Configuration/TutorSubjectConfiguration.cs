using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class TutorSubjectConfiguration : IEntityTypeConfiguration<TutorSubject>
{
	public void Configure(EntityTypeBuilder<TutorSubject> builder)
	{
		builder.ToTable("tutor_subject");

		builder.HasKey(ts => ts.TutorSubjectId);
		builder.Property(ts => ts.TutorSubjectId)
			.HasColumnName("tutor_subject_id")
			.UseIdentityAlwaysColumn();

		builder.Property(ts => ts.TutorId)
			.HasColumnName("tutor_id")
			.IsRequired();

		builder.Property(ts => ts.SubjectId)
			.HasColumnName("subject_id")
			.IsRequired();

		builder.Property(ts => ts.LevelId)
			.HasColumnName("level_id")
			.IsRequired();

		builder.Property(ts => ts.HourlyRate)
			.HasColumnName("hourly_rate")
			.HasPrecision(8, 2)
			.IsRequired();

		builder.HasIndex(ts => new { ts.TutorId, ts.SubjectId, ts.LevelId })
			.IsUnique()
			.HasDatabaseName("ts_unique_combination");

		builder.HasOne(ts => ts.Tutor)
			.WithMany(t => t.TutorSubjects)
			.HasForeignKey(ts => ts.TutorId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(ts => ts.Subject)
			.WithMany(s => s.TutorSubjects)
			.HasForeignKey(ts => ts.SubjectId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(ts => ts.TeachingLevel)
			.WithMany(l => l.TutorSubjects)
			.HasForeignKey(ts => ts.LevelId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
