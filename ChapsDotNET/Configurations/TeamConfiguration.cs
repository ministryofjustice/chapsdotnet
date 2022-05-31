using System;
using ChapsDotNET.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Configurations
{
	public class TeamConfiguration : IEntityTypeConfiguration<Team>
	{

        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(a => new { a.TeamID });
            builder.Property(e => e.Acronym).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.email).IsRequired().HasMaxLength(80);

        }
    }
}

