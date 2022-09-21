using System;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
	public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
		public void Configure(EntityTypeBuilder<Report> builder)
		{
            builder.HasKey(e => e.ReportId);
			builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
			builder.Property(e => e.Description).HasMaxLength(200);
            builder.Property(e => e.LongDescription).HasMaxLength(1000);
        }
    }
}
