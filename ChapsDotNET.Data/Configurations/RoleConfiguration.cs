using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {


        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.strength);
            builder.Property(e => e.strength).ValueGeneratedNever();
            builder.Property(e => e.Detail).IsRequired().HasMaxLength(20);

        }
    }
}

