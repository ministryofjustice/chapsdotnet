using ChapsDotNET.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Configurations
{
    public class LookUpModelConfiguration : IEntityTypeConfiguration<LookUpModel>
	{

        public void Configure(EntityTypeBuilder<LookUpModel> builder)
        {
            builder.Property(e => e.deactivatedBy).HasMaxLength(50);

        }
    }
}

