using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutoringPlatform.Models.Entities;

namespace TutoringPlatform.Models.Configuration;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
	public void Configure(EntityTypeBuilder<City> builder)
	{
		builder.ToTable("city");

		builder.HasKey(c => c.CityId);
		builder.Property(c => c.CityId)
			.HasColumnName("city_id")
			.UseIdentityAlwaysColumn();

		builder.Property(c => c.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(c => c.Region)
			.HasColumnName("region")
			.HasMaxLength(100);

		builder.Property(c => c.Country)
			.HasColumnName("country")
			.HasMaxLength(100)
			.IsRequired()
			.HasDefaultValue("Україна");

		builder.HasIndex(c => new { c.Name, c.Region, c.Country })
			.IsUnique()
			.HasDatabaseName("city_unique_location");
	}
}
