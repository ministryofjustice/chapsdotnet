using System;
using ChapsDotNET.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Configurations
{
	public class CorrespondenceTypeConfiguration : IEntityTypeConfiguration<CorrespondenceType>
	{
		
        public void Configure(EntityTypeBuilder<CorrespondenceType> builder)
        {
            builder.HasKey(e => e.CorrespondenceTypeID);
            builder.Property(e => e.Acronym).IsRequired().HasMaxLength(3);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(30);

        }
    }
}

