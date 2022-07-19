using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
    public class PublicHolidayConfiguration : IEntityTypeConfiguration<PublicHoliday>
    {
        public void Configure(EntityTypeBuilder<PublicHoliday> builder)
        {
            builder.HasKey(e => e.PublicHolidayID);
            builder.Property(e => e.Description).HasMaxLength(30);
        }
    }
}
