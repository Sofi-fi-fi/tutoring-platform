using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
	public void Configure(EntityTypeBuilder<Student> builder)
	{
		builder.ToTable("student");

		builder.HasKey(s => s.StudentId);
		builder.Property(s => s.StudentId).HasColumnName("student_id");

		builder.Property(s => s.CityId).HasColumnName("city_id");
		builder.Property(s => s.SchoolGrade).HasColumnName("school_grade");

		builder.HasIndex(s => s.CityId).HasDatabaseName("idx_student_city_id");

		builder.HasOne(s => s.City)
			.WithMany(c => c.Students)
			.HasForeignKey(s => s.CityId)
			.HasConstraintName("student_city_fk")
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasOne(s => s.User)
			.WithOne(u => u.Student)
			.HasForeignKey<Student>(s => s.StudentId)
			.HasConstraintName("student_user_fk")
			.OnDelete(DeleteBehavior.Cascade);
	}
}
