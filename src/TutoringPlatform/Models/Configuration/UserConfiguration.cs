using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("user");

		builder.HasKey(u => u.UserId);
		builder.Property(u => u.UserId)
			.HasColumnName("user_id")
			.UseIdentityAlwaysColumn();

		builder.Property(u => u.FirstName)
			.HasColumnName("first_name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(u => u.LastName)
			.HasColumnName("last_name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(u => u.Email)
			.HasColumnName("email")
			.HasMaxLength(255)
			.IsRequired();

		builder.HasIndex(u => u.Email).IsUnique();

		builder.Property(u => u.PasswordHash)
			.HasColumnName("password_hash")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(u => u.Phone)
			.HasColumnName("phone")
			.HasMaxLength(20);

		builder.Property(u => u.UserType)
			.HasColumnName("user_type")
			.HasConversion<string>()
			.IsRequired();

		builder.Property(u => u.DateOfBirth)
			.HasColumnName("date_of_birth");

		builder.Property(u => u.RegistrationDate)
			.HasColumnName("registration_date")
			.HasDefaultValueSql("CURRENT_TIMESTAMP");
	}
}
