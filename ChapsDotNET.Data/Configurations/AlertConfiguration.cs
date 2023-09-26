using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ChapsDotNET.Data.Configurations
{
	public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.HasKey(e => e.AlertID);
            builder.Property(e => e.Live).IsRequired();
            builder.Property(e => e.EventStart).IsRequired();
            builder.Property(e => e.RaisedHours).IsRequired();
            builder.Property(e => e.WarnStart).IsRequired();
            builder.Property(e => e.Message).IsRequired().HasMaxLength(200);

        }
    }
}

