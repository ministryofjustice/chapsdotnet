using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
	public class MPConfiguration : IEntityTypeConfiguration<MP>
	{
		public void Configure(EntityTypeBuilder<MP> builder)
		{
			builder.HasKey(e => e.MPID);
            builder.Property(e => e.Surname).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FirstNames).HasMaxLength(50);
            builder.Property(e => e.AddressLine1).HasMaxLength(100);
            builder.Property(e => e.AddressLine2).HasMaxLength(100);
			builder.Property(e => e.AddressLine3).HasMaxLength(100);
			builder.Property(e => e.Town).HasMaxLength(100);
			builder.Property(e => e.County).HasMaxLength(100);
			builder.Property(e => e.Postcode).HasMaxLength(10);
			builder.Property(e => e.Email).HasMaxLength(80);
			builder.Property(e => e.RtHon).IsRequired();
			builder.Property(e => e.Suffix).HasMaxLength(20);
		}
	}
}
