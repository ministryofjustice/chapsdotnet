using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
    public class StageConfiguration : IEntityTypeConfiguration<Stage>
    {
        public void Configure(EntityTypeBuilder<Stage> builder)
        {
            builder.HasKey(a => new { a.StageID });
            builder.Property(e => e.Name).IsRequired().HasMaxLength(40);
            builder.Property(e => e.CanBeAssignedToTeamID).IsRequired();
        }
    }
}

