using System;
using ChapsDotNET.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserID);
            builder.HasOne(a => a.Role).WithMany(b => b.Users).HasForeignKey(c => c.RoleStrength);
            builder.HasOne(a => a.Team).WithMany(b => b.Users).HasForeignKey(c => c.TeamID);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(150);
            builder.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.RoleStrength).IsRequired();
            builder.Property(e => e.email).HasMaxLength(80);

        }
    }
}

