using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
	public class MoJMinisterConfiguration : IEntityTypeConfiguration<MoJMinister>
	{
		public void Configure(EntityTypeBuilder<MoJMinister> builder)
		{
            builder.HasKey(e => e.MoJMinisterID);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.prefix).HasMaxLength(30);
            builder.Property(e => e.suffix).HasMaxLength(20);
            builder.Property(e => e.Rank).HasMaxLength(150);
		}
	}
}
